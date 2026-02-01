# SharedAssemblyInfo.cs - Centralized Versioning

## Overview

This solution uses `SharedAssemblyInfo.cs` at the solution root to manage common assembly attributes across all projects. This approach reduces duplication and ensures consistent versioning across all assemblies.

## Structure

### SharedAssemblyInfo.cs (Solution Root)

Contains **common attributes** shared by all projects:
- `AssemblyCompany`
- `AssemblyProduct`
- `AssemblyCopyright`
- `AssemblyTrademark`
- `AssemblyCulture`
- `ComVisible`
- **Version attributes** (synchronized with Directory.Build.props):
  - `AssemblyVersion`
  - `AssemblyFileVersion`
  - `AssemblyInformationalVersion`

### Project-Specific AssemblyInfo.cs

Each project's `Properties\AssemblyInfo.cs` contains **only project-specific attributes**:
- `AssemblyTitle` - Unique name for the project
- `AssemblyDescription` - Project-specific description
- `AssemblyConfiguration`
- `Guid` - COM interop GUID (if needed)

## How It Works

### Linking SharedAssemblyInfo.cs

Each project includes a reference to `SharedAssemblyInfo.cs` in its `.csproj` file:

```xml
<ItemGroup>
  <Compile Include="..\SharedAssemblyInfo.cs">
    <Link>Properties\SharedAssemblyInfo.cs</Link>
  </Compile>
  <Compile Include="Properties\AssemblyInfo.cs" />
  <!-- other files -->
</ItemGroup>
```

The `<Link>` element makes the file appear in the project's Properties folder in Solution Explorer, even though it's physically located at the solution root.

## Projects Using SharedAssemblyInfo

1. **WebApplicationSemVer** (Main web application)
   - Title: "WebApplicationSemVer"
   - Description: "ASP.NET MVC Web Application with Semantic Versioning"

2. **Database2** (.NET Framework class library)
   - Title: "Database2"
   - Description: "Database library for product data management"

3. **Database** (.NET Standard 2.0 class library)
   - Uses SDK-style project format
   - Versioning handled by Directory.Build.props

## Version Management

### Single Source of Truth

Versions are defined in **three synchronized locations**:

1. **Directory.Build.props** - MSBuild property definitions
2. **SharedAssemblyInfo.cs** - Assembly attribute definitions
3. **azure-pipelines.yml** - CI/CD pipeline version bumping

### Manual Version Update Process

When updating the version manually (local development):

1. Update `Directory.Build.props`:
   ```xml
   <Version>1.2.0</Version>
   ```

2. Update `SharedAssemblyInfo.cs`:
   ```csharp
   [assembly: AssemblyVersion("1.2.0.0")]
   [assembly: AssemblyFileVersion("1.2.0.0")]
   [assembly: AssemblyInformationalVersion("1.2.0")]
   ```

3. Rebuild solution

### Automated Version Update (CI/CD)

The Azure DevOps pipeline (`azure-pipelines.yml`) automatically:
1. Reads version from `Directory.Build.props`
2. Increments patch version (or major/minor based on commit message)
3. Updates both `Directory.Build.props` AND `SharedAssemblyInfo.cs`
4. Commits changes back to the repository

## Benefits

### ? Advantages

1. **Single Source of Version**: All assemblies share the same version number
2. **Reduced Duplication**: Common attributes defined once
3. **Easy Maintenance**: Update copyright year in one place
4. **Consistency**: All projects guaranteed to have matching metadata
5. **CI/CD Friendly**: Automated version bumping updates both files

### ?? Project-Specific Flexibility

Each project can still customize:
- Assembly title and description
- Configuration settings
- COM GUIDs
- Any other project-specific attributes

## Migration Checklist

When adding a new project to use SharedAssemblyInfo:

- [ ] Add link to `SharedAssemblyInfo.cs` in `.csproj`:
  ```xml
  <Compile Include="..\SharedAssemblyInfo.cs">
    <Link>Properties\SharedAssemblyInfo.cs</Link>
  </Compile>
  ```

- [ ] Remove duplicate attributes from `Properties\AssemblyInfo.cs`:
  - Remove: Company, Product, Copyright, Trademark, Culture
  - Remove: ComVisible
  - Remove: Version, FileVersion, InformationalVersion
  
- [ ] Keep only project-specific attributes:
  - AssemblyTitle
  - AssemblyDescription
  - AssemblyConfiguration
  - Guid (if needed for COM)

- [ ] Verify build succeeds

## Troubleshooting

### Duplicate Attribute Errors

If you see compilation errors like:
```
Duplicate 'AssemblyVersion' attribute
```

**Solution**: The attribute exists in both `AssemblyInfo.cs` and `SharedAssemblyInfo.cs`. Remove it from the project-specific `AssemblyInfo.cs`.

### Version Mismatch

If assemblies show different versions:

1. Clean and rebuild solution
2. Verify `SharedAssemblyInfo.cs` is properly linked in all `.csproj` files
3. Check that project-specific `AssemblyInfo.cs` files don't override version attributes

## References

- [Microsoft Docs: Assembly Attributes](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.assemblyattribute)
- [MSBuild Link Item Metadata](https://docs.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-items#link)
- Project documentation: `Directory.Build.props` usage
