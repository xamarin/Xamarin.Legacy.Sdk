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
* .NET 6 SDKs and any other dependencies as described [here](https://github.com/xamarin/net6-samples).

## TODO

Not implemented yet:

* Xamarin.Android binding projects
* Xamarin.iOS support
