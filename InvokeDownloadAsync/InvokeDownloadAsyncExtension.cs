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

        const string moduleScript = "export const f=()=>navigator.onLine;";
        await using var inlineJsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "data:text/javascript;charset=utf-8," + Uri.EscapeDataString(moduleScript));
        var isOnLine = await inlineJsModule.InvokeAsync<bool>("f");

        var scriptPath = "./_content/Toolbelt.Blazor.InvokeDownloadAsync/script.min.js";
        if (isOnLine) scriptPath += $"?v={version}";

        await using var module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", scriptPath);
        await module.InvokeVoidAsync("invokeDownload", fileName, contentType, contents);
    }
}
