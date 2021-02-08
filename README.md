# Xamarin.Legacy.Sdk

Starting from a .NET 6 project, adds the ability to target legacy
Xamarin target frameworks such as `monoandroid11.0` or
`xamarin.ios10`. *Not fully supported.*

## What is this? When would I need it?

You only need `Xamarin.Legacy.Sdk` if you have a NuGet package that
needs to target "legacy" Xamarin and .NET 6 *at the same time*.

*NOTE: If you are looking for general information about Xamarin and
.NET 6. You might start with the [net6-samples][net6-samples] Github
repository instead.*

Xamarin components like [AndroidX][androidx] or [Google Play
Services][gps] are prime examples that need to use
`Xamarin.Legacy.Sdk`.

For example, the current `Xamarin.AndroidX.AppCompat.nupkg` includes:

* lib\monoandroid90\
  * Xamarin.AndroidX.AppCompat.dll

Note that the `monoandroid90` assembly *may* continue to fully work in
.NET 6. This, however, brings some compatibility baggage along with
it. For example, `Xamarin.AndroidX.AppCompat.dll` will have a
reference to `mscorlib.dll`. `mscorlib.dll` does not exist in .NET 6,
and types in `mscorlib.dll` are [forwarded][type-forwards] to the
appropriate .NET 6 BCL assembly. The way for
`Xamarin.AndroidX.AppCompat.dll` to fully support .NET 6 is to
actually be compiled *against* .NET 6.

If the package needs to continue shipping `monoandroid90` but *also*
include full support for .NET 6, the package could include additional
target frameworks:

* lib\monoandroid90\
  * Xamarin.AndroidX.AppCompat.dll
* lib\net6.0-android30.0\
  * Xamarin.AndroidX.AppCompat.dll

Previously, there was not a "nice" way to produce the above package.
It would involve multiple `.csproj` files and a `.nuspec` that
combines the files together.

Xamarin.Legacy.Sdk solves this problem by:

* Starting with a .NET 6 project that produces a `net6.0` class
  library -- built via `dotnet build`.
* Using `Xamarin.Legacy.Sdk` allows the .NET 6 project to add
  additional `$(TargetFrameworks)` such as `monoandroid11.0` or
  `xamarin.ios10`.
* This effectively imports `Xamarin.Android/iOS.CSharp.targets`
  running the existing Xamarin MSBuild tasks & targets under `dotnet
  build`.

[net6-samples]: https://github.com/xamarin/net6-samples
[androidx]: https://github.com/xamarin/AndroidX
[gps]: https://github.com/xamarin/GooglePlayServicesComponents
[type-forwards]: https://docs.microsoft.com/dotnet/standard/assembly/type-forwarding

### Getting started

`Xamarin.Legacy.Sdk` allows you to create a class library such as:

```xml
<Project Sdk="Xamarin.Legacy.Sdk">
  <PropertyGroup>
    <TargetFrameworks>monoandroid11.0;xamarin.ios10;net6.0-android;net6.0-ios</TargetFrameworks>
  </PropertyGroup>
</Project>
```

You will also need either include a `global.json` with:

```json
{
  "msbuild-sdks": {
    "Xamarin.Legacy.Sdk": "0.1.0-alpha1"
  }
}
```

Or specify the version inline:

```xml
<Project Sdk="Xamarin.Legacy.Sdk/0.1.0-alpha1">
```

To setup a binding project instead of a class library, simply set
`<IsBindingProject>true</IsBindingProject>` in your `.csproj` file.

### What about [MSBuild.Sdk.Extras][sdkextras]?

We considered adding to [MSBuild.Sdk.Extras][sdkextras], however, a few things would make this story... complicated.

`Xamarin.Legacy.Sdk` emulates all behavior of .NET 6. This means all MSBuild defaults will be applied, such as:

* `$(DefineConstants)` includes `ANDROID` and `IOS` defines as are
  included in .NET 6 by default.
* Default wildcards from [`AutoImport.props`][autoimport].
* MSBuild property defaults from
  [`DefaultProperties.targets`][defaultprops].

This would certainly introduce breaking changes to [MSBuild.Sdk.Extras][sdkextras].

[sdkextras]: https://github.com/novotnyllc/MSBuildSdkExtras
[autoimport]: https://github.com/xamarin/xamarin-android/blob/15b40af7d62e0e2003d2a009576834a71967dbb2/src/Xamarin.Android.Build.Tasks/Microsoft.Android.Sdk/Sdk/AutoImport.props
[defaultprops]: https://github.com/xamarin/xamarin-android/blob/15b40af7d62e0e2003d2a009576834a71967dbb2/src/Xamarin.Android.Build.Tasks/Microsoft.Android.Sdk/targets/Microsoft.Android.Sdk.DefaultProperties.targets

## Samples

* `Hello`: a simple class library targeting Xamarin.iOS,
  Xamarin.Android, and .NET 6
* `JavaBinding`: a Xamarin.Android binding for
  [com.google.code.gson][gson] based off of the
  [GoogleGson][xamaringson] Xamarin component.

[gson]: https://mvnrepository.com/artifact/com.google.code.gson/gson/2.8.5
[xamaringson]: https://github.com/xamarin/XamarinComponents/tree/master/Android/GoogleGson/source/GoogleGson

## Installation requirements

You will need:

* .NET 6 SDKs and any other dependencies as described by the [.NET 6
  samples repo][net6-samples].
* At least Visual Studio 2019 16.9 Preview 4 or higher for IDE support
  for .NET 6.
* The Xamarin workload from the Visual Studio installer.

*NOTE: that at least Xamarin.Android 11.2.99.43 is needed for Java
bindings, which is a nightly build.*

View [azure-pipelines.yml](azure-pipelines.yml) for known good builds
and download links.

## Troubleshooting

### Notes for Windows

`dotnet build` command-line with .NET 6 will have a new enough MSBuild for this to work.

Otherwise, you will at least need Visual Studio 2019 16.9 which is currently in preview to use:

```cmd
> "C:\Program Files (x86)\Microsoft Visual Studio\2019\Preview\MSBuild\Current\Bin\MSBuild.exe" -version
```

You will also need to enable a feature-flag to enable .NET 6 workloads. In an administrator command prompt:

```cmd
cd "C:\Program Files (x86)\Microsoft Visual Studio\2019\Preview\MSBuild\Current\Bin\SdkResolvers\Microsoft.DotNet.MSBuildSdkResolver"
echo > EnableWorkloadResolver.sentinel
```

This will create an empty file.

#### Microsoft.Android.Sdk not installed

If you hit the error:

```
The following workload packs were not installed: Microsoft.Android.Sdk
```

Temporarily, you can make a symbolic link to workaround this. In an administrator command prompt:

```cmd
> mklink /J "C:\Program Files\dotnet\packs\Microsoft.Android.Sdk" "C:\Program Files\dotnet\packs\Microsoft.Android.Sdk.win-x64"
Junction created for C:\Program Files\dotnet\packs\Microsoft.Android.Sdk <<===>> C:\Program Files\dotnet\packs\Microsoft.Android.Sdk.win-x64
```

This workaround should no longer be needed when the MSBuild shipped in Visual Studio is updated.

## Notes for macOS

The version of MSBuild shipped with Mono & VS for Mac is not currently new enough to build .NET 6 projects. For now, you will need to use `dotnet build` at the command-line.

If you hit the error:

```
error : /usr/local/share/dotnet/sdk/6.0.100-alpha.1.20562.2/Sdks/Microsoft.Android.Sdk/Sdk not found. Check that a recent enough .NET SDK is installed and/or increase the version specified in global.json.
```

Verify you are using `dotnet build` have the required [.NET 6][net6-samples] packages installed.

#### libMonoPosixHelper.dylib

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

This simply copies `libMonoPosixHelper.dylib` to an additional location so it can be loaded when running under .NET 6.

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft 
trademarks or logos is subject to and must follow 
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.
