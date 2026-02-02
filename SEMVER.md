# Semantic Versioning Guide

This repository uses **Semantic Versioning 2.0.0** for all releases and builds.

## Version Format

All versions are created and incremented by GitHub Actions and Azure DevOps (ADO) pipelines.

### Tag Format

- **Format:** `vMAJOR.MINOR.PATCH` (example: `v2.0.33`)
- **Branch:** Tags are exclusively created by the `main` or `release/*` branches using the `Directory.Build.props` file as the **exclusive source of truth for all version numbers**.

## Version Components

### Key Types

- **AssemblyVersion** - Unique `VersionMajor` and `VersionMinor` (no `VersionPatch`). Properties used by .NET as the **exclusive source of truth for all assembly load identities** across all projects.
  - Example: `2.0.0.0`
  - Stable, only changes for breaking changes (MAJOR) or new features (MINOR)

- **FileVersion** - Combines `VersionMajor`, `VersionMinor`, and `VersionPatch` (from `Directory.Build.props`). No additional suffix required.
  - Example: `2.0.33.0`
  - Used for diagnostics and troubleshooting

- **InformationalVersion** - Contains all semver fields but does NOT include `SourceRevisionId` or `AssemblyInfoVersion` unless custom additions are made.
  - Example: `2.0.33` or `2.0.33-beta`

### Version Incrementing Rules

1. **MAJOR** version when you make incompatible API changes
   - Example: `1.0.0` -> `2.0.0`
   
2. **MINOR** version when you add functionality in a backwards compatible manner
   - Example: `1.0.0` -> `1.1.0`

3. **PATCH** version when you make backwards compatible bug fixes
   - Example: `1.0.0` -> `1.0.1`

## Directory.Build.props - Single Source of Truth

All version information is centralized in `Directory.Build.props`:

```xml
<VersionMajor>2</VersionMajor>
<VersionMinor>0</VersionMinor>
<VersionPatch>0</VersionPatch>  <!-- Auto-incremented by CI/CD -->
```

### How Versioning Works

1. **Local Development:**
   - `VersionMajor` and `VersionMinor` are set manually in `Directory.Build.props`
   - `VersionPatch` defaults to `0`

2. **CI/CD Builds:**
   - Pipeline reads `VersionMajor` and `VersionMinor` from `Directory.Build.props`
   - `VersionPatch` is auto-incremented using build counter (e.g., `GITHUB_RUN_NUMBER`)
   - No manual file edits required

3. **Classic .NET Framework Projects:**
   - Use MSBuild target `GenerateVersionInfo` to auto-generate version attributes
   - Version values come from `Directory.Build.props`
   - See `docs/SharedAssemblyInfo-Usage.md` for setup details

4. **SDK-Style Projects:**
   - Automatically use version from `Directory.Build.props`
   - No additional configuration needed

## GitHub Actions Workflow

### Version Calculation

```yaml
- name: Calculate version
  id: version
  run: |
    $versionMajor = "${{ steps.read_version.outputs.MAJOR }}"
    $versionMinor = "${{ steps.read_version.outputs.MINOR }}"
    $versionPatch = $env:GITHUB_RUN_NUMBER
    $semVer = "$versionMajor.$versionMinor.$versionPatch"
```

### Build Artifacts

- Artifacts are named with the computed `SEMVER`
- Example: `WebApplicationSemVer-2.0.33`

## Releases and Tags

### Creating a Release

1. **Update version in `Directory.Build.props`:**
   ```bash
   # For a new minor version (e.g., 2.0 -> 2.1)
   git checkout main
   # Edit Directory.Build.props: Change VersionMinor to 1
   git add Directory.Build.props
   git commit -m "chore: Bump version to 2.1"
   git push origin main
   ```

2. **Pipeline automatically:**
   - Reads `2.1` from `Directory.Build.props`
   - Sets `VersionPatch` to build number (e.g., `47`)
   - Creates tag `v2.1.47`
   - Builds all projects with version `2.1.47`

### Tag Naming Convention

- **Stable releases:** `v2.1.47`
- **Pre-release (if using suffix):** `v2.1.47-beta`, `v2.1.47-rc1`

## Best Practices

### DO

- **Update only `Directory.Build.props`** when changing MAJOR or MINOR versions
- **Let CI/CD auto-increment PATCH** using build counters
- **Use stable AssemblyVersion** (MAJOR.MINOR.0.0) to avoid binding redirects
- **Use FileVersion for diagnostics** - it captures the exact build number
- **Tag releases from `main` branch** for production builds

### DON'T

- **Don't manually edit `AssemblyInfo.cs`** - versions are auto-generated
- **Don't manually edit `SharedAssemblyInfo.cs`** for version attributes
- **Don't change `VersionPatch` in `Directory.Build.props`** before committing - let CI/CD handle it
- **Don't create version tags manually** - let the pipeline create them

## Troubleshooting

### Different versions in DLLs

**Solution:**
1. Verify `Directory.Build.props` exists at solution root
2. For classic projects: Verify `GenerateVersionInfo` MSBuild target exists in `.csproj`
3. Clean and rebuild: `dotnet clean && dotnet build`

### CI/CD not creating tags

**Solution:**
1. Ensure workflow has permissions to create tags
2. Check that build is running on `main` or `release/*` branch
3. Verify `Directory.Build.props` is being updated correctly in the pipeline

### Version shows 0.0.0.0

**Solution:**
1. Classic projects: Add the `GenerateVersionInfo` MSBuild target to `.csproj`
2. Remove any hardcoded version attributes from `AssemblyInfo.cs` files

## Version History Example

| Version | Type | Changes |
|---------|------|---------|
| **2.0.0** | Major | Breaking changes - migrated to new versioning system |
| **2.1.0** | Minor | Added new features (backwards compatible) |
| **2.1.1** | Patch | Bug fixes only |
| **2.2.0-beta** | Pre-release | Beta release for testing |

---

**See also:**
- [Directory.Build.props](./Directory.Build.props) - Version configuration
- [SharedAssemblyInfo Usage](./docs/SharedAssemblyInfo-Usage.md) - Setup guide
- [GitHub Actions Workflow](./.github/workflows/build.yml) - CI/CD pipeline
- [Semantic Versioning 2.0.0](https://semver.org/) - Official specification
