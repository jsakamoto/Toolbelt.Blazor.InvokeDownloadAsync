# Blazor InvokeDownloadAsync [![NuGet Package](https://img.shields.io/nuget/v/Toolbelt.Blazor.InvokeDownloadAsync.svg)](https://www.nuget.org/packages/Toolbelt.Blazor.InvokeDownloadAsync)

## Summary

This NuGet package simply adds the `InvokeDownloadAsync()` extension method on your Blazor apps.

```csharp
@using Toolbelt.Blazor.Extensions
@inject IJSRuntime JSRuntime
...
@code
{
  private async Task BeginDownloadAsync()
  {
    ...
    await this.JSRuntime.InvokeDownloadAsync(
      "Foo.png",
      "image/png",
      pictureBytes);
  }
}
```

## Usage

Install this NuGet package to your Blazor application project.

```shell
dotnet add package Toolbelt.Blazor.InvokeDownloadAsync
```

After that, you can use the `InvokeDownloadAsync()` extension method for the `IJSRuntime` interface if you have opened the `Toolbelt.Blazor.Extensions` namespace.

### Syntax

```csharp
ValueTask InvokeDownloadAsync(
      this IJSRuntime jsRuntime,
      string fileName,
      string contentType,
      byte[] contentBytes);
```

## Release notes

Release notes is [here](https://github.com/jsakamoto/Toolbelt.Blazor.InvokeDownloadAsync/blob/main/RELEASE-NOTES.txt).

## License

[MIT License](https://github.com/jsakamoto/Toolbelt.Blazor.InvokeDownloadAsync/blob/main/LICENSE)