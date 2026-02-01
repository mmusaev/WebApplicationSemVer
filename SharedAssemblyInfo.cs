using System.Reflection;
using System.Runtime.InteropServices;

// Shared assembly information across all projects in the solution
// Common attributes that apply to all assemblies

[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("WebApplicationSemVer")]
[assembly: AssemblyCopyright("Copyright ©  2026")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// Version information - NOTE: Also defined in Directory.Build.props
// For local development: Manually sync these with Directory.Build.props when bumping version
// For CI/CD builds: azure-pipelines.yml automatically updates these values
[assembly: AssemblyVersion("1.1.0.0")]
[assembly: AssemblyFileVersion("1.1.0.0")]
[assembly: AssemblyInformationalVersion("1.1.0")]
