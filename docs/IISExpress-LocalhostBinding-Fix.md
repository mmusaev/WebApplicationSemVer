# Fixing IIS Express Localhost Binding Issues

## Problem
When running an ASP.NET Framework web application in Visual Studio with IIS Express, you may encounter connection errors such as:
- `ERR_CONNECTION_RESET`
- `Unable to connect to localhost`
- SSL certificate errors
- The site loads on `127.0.0.1` but not on `localhost`

## Solution: IISExpressAdminCmd.exe

IIS Express includes a built-in administrative command-line tool specifically designed to manage SSL certificates and URL bindings for localhost.

### Command to Fix Localhost Binding

Open an **Administrator Command Prompt** or **Administrator PowerShell** and run:

```powershell
"%ProgramFiles%\IIS Express\IisExpressAdminCmd.exe" setupsslUrl -url:https://localhost:44348/ -UseSelfSigned
```

**Note:** Replace `44348` with your application's SSL port number if different.

### What This Command Does

- **setupsslUrl**: Configures SSL URL reservation for IIS Express
- **-url:https://localhost:44348/**: Specifies the URL to bind
- **-UseSelfSigned**: Creates and uses a self-signed SSL certificate for development

### Additional Commands

#### Remove an Existing Binding

If you need to remove a binding first (useful when troubleshooting):

```powershell
"%ProgramFiles%\IIS Express\IisExpressAdminCmd.exe" deletesslUrl -url:https://localhost:44348/
```

#### Setup a New Binding with Self-Signed Certificate

After removing, recreate the binding:

```powershell
"%ProgramFiles%\IIS Express\IisExpressAdminCmd.exe" setupsslUrl -url:https://localhost:44348/ -UseSelfSigned
```

### Alternative Solutions

#### 1. Run Visual Studio as Administrator
The simplest solution is to run Visual Studio as Administrator, which automatically handles URL reservations.

1. Close Visual Studio
2. Right-click Visual Studio icon ? "Run as administrator"
3. Open your solution and press F5

#### 2. Using netsh (Alternative Method)
If `IisExpressAdminCmd.exe` doesn't work, you can use `netsh`:

```powershell
netsh http add urlacl url=https://localhost:44348/ user=everyone
```

### Verifying the Fix

After running the command:

1. Restart Visual Studio (if it was already running)
2. Press F5 to run your application
3. Navigate to `https://localhost:44348/`
4. You should now see your application without connection errors

### Finding Your SSL Port

If you don't know your SSL port, check:

1. Right-click your web project ? Properties
2. Go to the **Web** tab
3. Look for "Project URL" under "Servers" section
4. The port number appears in the URL (e.g., `https://localhost:44348/`)

### Common Issues

**"Access is denied" error:**
- Make sure you're running the command prompt or PowerShell as Administrator

**Certificate trust warnings in browser:**
- Self-signed certificates will show browser warnings
- Click "Advanced" ? "Proceed to localhost" (development only!)
- Or install the IIS Express certificate in your Trusted Root store

**Port already in use:**
- Change the port number in your project properties
- Update the command with the new port number

## Summary

For this project (`WebApplicationSemVer`), the issue was resolved by running:

```powershell
"%ProgramFiles%\IIS Express\IisExpressAdminCmd.exe" setupsslUrl -url:https://localhost:44348/ -UseSelfSigned
```

This configured IIS Express to properly bind to `https://localhost:44348/` with a self-signed SSL certificate, allowing the application to run without connection errors.

## References

- [IIS Express Overview - Microsoft Docs](https://docs.microsoft.com/en-us/iis/extensions/introduction-to-iis-express/)
- [Configure SSL in IIS Express](https://docs.microsoft.com/en-us/iis/extensions/using-iis-express/handling-url-binding-failures-in-iis-express)
