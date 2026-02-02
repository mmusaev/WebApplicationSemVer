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

## What SharedAssemblyInfo.cs Contains

**Non-version attributes only** (shared across all projects):

```csharp
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("WebApplicationSemVer")]
[assembly: AssemblyCopyright("Copyright © 2026")]
[assembly: ComVisible(false)]

// Version attributes are NO LONGER here!
// They are auto-generated from Directory.Build.props via MSBuild target
```

## How Versioning Works

### 1. Directory.Build.props (Single Source of Truth)
All version info is defined here:

```xml
<VersionMajor>2</VersionMajor>
<VersionMinor>0</VersionMinor>
<VersionPatch>0</VersionPatch>

<!-- Computed automatically -->
<AssemblyVersion>2.0.0.0</AssemblyVersion>
<FileVersion>2.0.0.0</FileVersion>
<InformationalVersion>2.0.0</InformationalVersion>
```

#### Understanding the Version Properties

| Property | Format | Purpose |
|----------|--------|---------|
| **AssemblyVersion** | `MAJOR.MINOR.0.0` | .NET runtime binding identity. Kept stable to avoid binding redirects. |
| **FileVersion** | `MAJOR.MINOR.PATCH.0` | Exact build number for diagnostics. Shown in file properties. |
| **InformationalVersion** | `MAJOR.MINOR.PATCH[-suffix]` | Full semantic version. May include prerelease suffix (e.g., `-beta`). |

#### Why AssemblyVersion is `2.0.0.0` (not `2.0.33.0`)

**This is intentional and a .NET best practice:**

- **Avoids binding redirects** - Assemblies with the same `AssemblyVersion` are considered compatible by the .NET runtime. Changing it for every build would require `app.config` binding redirects.
- **Enables side-by-side compatibility** - Different patch builds can load without conflicts.
- **Semantic meaning** - Only changes when the API contract changes (MAJOR = breaking change, MINOR = new feature).
- **FileVersion captures the details** - Use `FileVersion` (e.g., `2.0.33.0`) to identify the exact build for troubleshooting.

**Example CI build #33:**
- `AssemblyVersion`: `2.0.0.0` (stable)
- `FileVersion`: `2.0.33.0` (exact build)
- `InformationalVersion`: `2.0.33` (semantic version)


### 2. SDK-Style Projects (.NET Standard/Core)
- Automatically use version from `Directory.Build.props`
- No additional setup required ✅

### 3. Classic .NET Framework Projects
- Use MSBuild target to **auto-generate** `VersionInfo.cs` at build time
- Version values come from `Directory.Build.props`
- See "How To Set Up Auto-Versioning" section below for setup

## Projects and Versioning Strategy

| Project | Type | Versioning Method |
|---------|------|-------------------|
| **WebApplicationSemVer** | ASP.NET MVC 4.8.1 | MSBuild auto-generates `VersionInfo.cs` |
| **Database2** | Class Library 4.8.1 | MSBuild auto-generates `VersionInfo.cs` |
| **Database** | .NET Standard 2.0 (SDK-style) | Automatic from `Directory.Build.props` |

**All projects reference `SharedAssemblyInfo.cs` for non-version attributes (Company, Product, Copyright).**

## How To Set Up Auto-Versioning for Classic .NET Framework Projects

For classic .NET Framework projects (non-SDK style), you need to add an MSBuild target to auto-generate version attributes from `Directory.Build.props`.

### Step 1: Link SharedAssemblyInfo.cs

Add this to your `.csproj` file:

```xml
<ItemGroup>
  <Compile Include="..\SharedAssemblyInfo.cs">
    <Link>Properties\SharedAssemblyInfo.cs</Link>
  </Compile>
  <Compile Include="Properties\AssemblyInfo.cs" />
</ItemGroup>
```

### Step 2: Add MSBuild Target for Auto-Generated Versions

**⚠️ CRITICAL:** Add this MSBuild target to **EACH** classic .NET Framework `.csproj` file **BEFORE** the `<Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />` line:

**Example projects that need this:**
- `WebApplicationSemVer\WebApplicationSemVer.csproj`
- `Database2\Database2.csproj`
- Any other classic .NET Framework project in your solution

```xml
<!-- Generate VersionInfo.cs with version attributes from MSBuild properties -->
<Target Name="GenerateVersionInfo" BeforeTargets="BeforeBuild">
  <WriteLinesToFile
    File="$(IntermediateOutputPath)VersionInfo.cs"
    Lines='[assembly: System.Reflection.AssemblyVersion("$(AssemblyVersion)")]&#xD;&#xA;[assembly: System.Reflection.AssemblyFileVersion("$(FileVersion)")]&#xD;&#xA;[assembly: System.Reflection.AssemblyInformationalVersion("$(InformationalVersion)")]'
    Overwrite="true" />
  <ItemGroup>
    <Compile Include="$(IntermediateOutputPath)VersionInfo.cs" />
  </ItemGroup>
</Target>
```

**What this does:**
- Before each build, MSBuild generates a `VersionInfo.cs` file with version attributes
- The version values come from `Directory.Build.props` ($(AssemblyVersion), $(FileVersion), $(InformationalVersion))
- The file is placed in the `obj\` folder and automatically compiled
- **Result:** Your classic project gets the same version as SDK-style projects automatically!

### Step 3: Remove Version Attributes from SharedAssemblyInfo.cs

In `SharedAssemblyInfo.cs`, **REMOVE** these lines (if present):

```csharp
// ❌ DELETE THESE - they are now auto-generated:
[assembly: AssemblyVersion("...")]
[assembly: AssemblyFileVersion("...")]
[assembly: AssemblyInformationalVersion("...")]
```

Keep only:
```csharp
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("WebApplicationSemVer")]
[assembly: AssemblyCopyright("Copyright © 2026")]
[assembly: ComVisible(false)]
```

### Step 4: Keep Project-Specific Attributes Only

In `Properties\AssemblyInfo.cs`, keep **ONLY** project-specific attributes:

```csharp
[assembly: AssemblyTitle("YourProjectName")]
[assembly: AssemblyDescription("Project description")]
[assembly: AssemblyConfiguration("")]
[assembly: Guid("your-guid-here")] // For COM interop
```

**⚠️ DO NOT include:** AssemblyCompany, AssemblyProduct, AssemblyCopyright, ComVisible, or **any version attributes**

### Step 5: Rebuild

```bash
dotnet clean
dotnet build
```

**Verify:** Check your DLL properties - the File Version should match the version in `Directory.Build.props`!

## How To Update Version

**Update only ONE file:** `Directory.Build.props`

### Local Development

Edit `Directory.Build.props`:

```xml
<VersionMajor>2</VersionMajor>
<VersionMinor>1</VersionMinor>
<VersionPatch>0</VersionPatch>
```

Commit:
```bash
git add Directory.Build.props
git commit -m "chore: Bump version to 2.1.0"
```

**That's it!** All projects will automatically use the new version on next build.

### CI/CD Pipeline (Azure DevOps / GitHub Actions)

The pipeline can auto-increment `VersionPatch` or set a build number:

**Example (Azure Pipelines):**
```yaml
- task: PowerShell@2
  inputs:
    targetType: 'inline'
    script: |
      $buildNumber = "$(Build.BuildId)"
      (Get-Content Directory.Build.props) -replace '<VersionPatch>0</VersionPatch>', "<VersionPatch>$buildNumber</VersionPatch>" | Set-Content Directory.Build.props
```

**Result:**
- Local builds: `2.1.0.0`
- CI/CD build #47: `2.1.47.0`
- All DLLs get the same version automatically!

## Troubleshooting

### Error: Duplicate 'AssemblyVersion' attribute

**Cause:** Version attribute exists in `SharedAssemblyInfo.cs` or `Properties\AssemblyInfo.cs` AND auto-generated `VersionInfo.cs`

**Fix:** 
1. Remove **all** version attributes from `SharedAssemblyInfo.cs`
2. Remove **all** version attributes from `Properties\AssemblyInfo.cs`:
   - `[assembly: AssemblyVersion(...)]`
   - `[assembly: AssemblyFileVersion(...)]`
   - `[assembly: AssemblyInformationalVersion(...)]`

### Where is the auto-generated VersionInfo.cs file?

**Location:** `obj\Debug\VersionInfo.cs` or `obj\Release\VersionInfo.cs`

This file is generated at build time and automatically included in compilation. You won't see it in Solution Explorer.

**To verify it was generated:**
```bash
# After building, check if the file exists:
dir obj\Debug\VersionInfo.cs
# or
dir obj\Release\VersionInfo.cs
```

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
