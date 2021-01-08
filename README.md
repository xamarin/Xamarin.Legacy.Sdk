# Xamarin.Legacy.Sdk

An unofficial MSBuild SDK for multitargeting for "legacy" Xamarin and .NET 6.

This allows you to create a class library such as:

```xml
<Project Sdk="Xamarin.Legacy.Sdk">
  <PropertyGroup>
    <TargetFrameworks>monoandroid11.0;net6.0-android</TargetFrameworks>
  </PropertyGroup>
</Project>
```

You will also need either include a `global.json` with:

```json
{
  "msbuild-sdks": {
    "Xamarin.Legacy.Sdk": "0.1.0"
  }
}
```

Or specify the version inline:

```xml
<Project Sdk="Xamarin.Legacy.Sdk/0.1.0">
```

## Installation requirements

You will need:

* At least Visual Studio 2019 16.9 or higher.
* Xamarin.Android from the Visual Studio installer.
* .NET 6 SDKs and any other dependencies as described [here][net6-samples].

## Notes for macOS

The version of MSBuild shipped with Mono & VS for Mac is not currently new enough to build .NET 6 projects. For now, you will need to use `dotnet build` at the command-line.

If you hit the error:

```
error : /usr/local/share/dotnet/sdk/6.0.100-alpha.1.20562.2/Sdks/Microsoft.Android.Sdk/Sdk not found. Check that a recent enough .NET SDK is installed and/or increase the version specified in global.json.
```

Verify you are using `dotnet build` have the required [.NET 6][net6-samples] packages installed.

### libMonoPosixHelper.dylib

If you hit the error:

```
error XACML7000: System.DllNotFoundException: Unable to load shared library 'MonoPosixHelper' or one of its dependencies.
In order to help diagnose loading problems, consider setting the DYLD_PRINT_LIBRARIES environment variable: dlopen(libMonoPosixHelper, 1): image not found 
```

A temporary workaround would be:

```bash
$ cd /Library/Frameworks/Xamarin.Android.framework/Versions/Current/lib/xbuild/Xamarin/Android/lib/host-Darwin/
$ sudo cp libMonoPosixHelper.dylib ../../
```

This simply copies to an additional location so it can be loaded when running under .NET 6.

## TODO

Not implemented yet:

* Xamarin.Android binding projects
* Xamarin.iOS support

[net6-samples]: https://github.com/xamarin/net6-samples
