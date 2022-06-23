using System.Reflection;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.Extensions;

public static class InvokeDownloadAsyncExtension
{
    public static async ValueTask InvokeDownloadAsync(this IJSRuntime jsRuntime, string fileName, string contentType, byte[] contents)
    {
        var assembly = typeof(InvokeDownloadAsyncExtension).Assembly;
        var version = assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? assembly.GetName().Version?.ToString() ?? "0.0.0";

        await using var module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", $"./_content/Toolbelt.Blazor.InvokeDownloadAsync/script.js?v={version}");
        await module.InvokeVoidAsync("invokeDownload", fileName, contentType, contents);
    }
}
