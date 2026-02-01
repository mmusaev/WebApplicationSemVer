# SharedAssemblyInfo.cs - Centralized Assembly Versioning

## Purpose

Single source of truth for version numbers and common assembly attributes shared across all .NET Framework projects in the solution.

**Benefits:**
- ✅ All projects share the same version automatically
- ✅ Update version in one place instead of multiple AssemblyInfo.cs files
- ✅ Eliminates version mismatch between assemblies
- ✅ Simplifies CI/CD version management

## Current Version: 2.0.0

## Location
```
C:\dev\WebApplicationSemVer\SharedAssemblyInfo.cs
```

## What It Contains

```csharp
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("WebApplicationSemVer")]
[assembly: AssemblyCopyright("Copyright © 2026")]
[assembly: ComVisible(false)]

// Synchronized with Directory.Build.props
[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0.0")]
[assembly: AssemblyInformationalVersion("2.0.0")]
```

## Projects Using SharedAssemblyInfo.cs

| Project | Type | Purpose |
|---------|------|---------|
| **WebApplicationSemVer** | ASP.NET MVC 4.8.1 | Main web application |
| **Database2** | Class Library 4.8.1 | Order management |

*Note: Database project uses SDK-style format and gets version from Directory.Build.props*

## How To Link SharedAssemblyInfo.cs to a Project

### Step 1: Edit Project File (.csproj)

Add this to your `.csproj` file:

```xml
<ItemGroup>
  <Compile Include="..\SharedAssemblyInfo.cs">
    <Link>Properties\SharedAssemblyInfo.cs</Link>
  </Compile>
  <Compile Include="Properties\AssemblyInfo.cs" />
</ItemGroup>
```

### Step 2: Keep Project-Specific Attributes Only

In `Properties\AssemblyInfo.cs`, keep only project-specific attributes:

```csharp
[assembly: AssemblyTitle("YourProjectName")]
[assembly: AssemblyDescription("Project description")]
[assembly: AssemblyConfiguration("")]
[assembly: Guid("your-guid-here")] // If needed
```

**Note:** Do not duplicate attributes from SharedAssemblyInfo.cs (Company, Product, Copyright, Version, etc.)

### Step 3: Rebuild

```bash
dotnet clean
dotnet build
```

## How To Update Version

Update **both files together**:

### 1. Update Directory.Build.props
```xml
<VersionMajor>2</VersionMajor>
<VersionMinor>1</VersionMinor>
<VersionPatch>0</VersionPatch>
```

### 2. Update SharedAssemblyInfo.cs
```csharp
[assembly: AssemblyVersion("2.1.0.0")]
[assembly: AssemblyFileVersion("2.1.0.0")]
[assembly: AssemblyInformationalVersion("2.1.0")]
```

### 3. Commit Both Files
```bash
git add Directory.Build.props SharedAssemblyInfo.cs
git commit -m "chore: Bump version to 2.1.0"
```

## Troubleshooting

### Error: Duplicate 'AssemblyVersion' attribute

**Cause:** Version attribute exists in both SharedAssemblyInfo.cs and project's AssemblyInfo.cs

**Fix:** Remove version attributes from `Properties\AssemblyInfo.cs`

### Projects Show Different Versions

**Fix:**
1. Clean solution: `dotnet clean`
2. Delete `bin` and `obj` folders
3. Verify SharedAssemblyInfo.cs is linked in `.csproj`
4. Rebuild: `dotnet build`

## Version History

| Version | Date | Changes |
|---------|------|---------|
| **2.0.0** | 2026-02-01 | Added Order management system |
| 1.1.0 | 2026-01-15 | Added semantic versioning |
| 1.0.0 | 2026-01-01 | Initial release |

## Semantic Versioning

- **MAJOR** (2.x.x) - Breaking changes
- **MINOR** (x.1.x) - New features, backward compatible
- **PATCH** (x.x.1) - Bug fixes only

---

**See also:**
- [Directory.Build.props](../Directory.Build.props) - MSBuild version properties
- [IIS Express Setup](./IISExpress-LocalhostBinding-Fix.md) - Local development setup
