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

## Features

### 1. 100% `eval()` free

This package uses the standard `InvokeAsync("import",...)` method calling to import helper JavaScript code that is written as an ES module. 100% `eval()` free. (Instead, this package can not use for apps on .NET Core 3.x.)

### 2. Minimal setup

This package is an extension method for the `IJSRuntime` interface. You can use the downloading feature immediately if there is a JavaScript runtime object. You don't need to write any code in your startup.

### 3. Small package size

This package is simple and small content size because this package doesn't have massive code optimized for performance.

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