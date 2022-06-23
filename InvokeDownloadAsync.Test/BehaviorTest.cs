using Toolbelt.Blazor.InvokeDownloadAsync.Test.Internals;

namespace Toolbelt.Blazor.InvokeDownloadAsync.Test;

public class BehaviorTest
{
    private static readonly Internals.TestContext _TestContext = new();

    private static readonly IEnumerable<HostingModel> HostingModels = new[] {
        HostingModel.Wasm,
        HostingModel.Server,
    };

    private static readonly IEnumerable<BlazorVersion> BlazorVersions = new[] {
        BlazorVersion.NET50,
        BlazorVersion.NET60,
    };

    public static object[][] TestCases { get; } = (
        from model in HostingModels
        from ver in BlazorVersions
        select new object[] { model, ver }).ToArray();

    [TestCaseSource(nameof(TestCases))]
    public async Task InvokeDownloadAsync_Test(HostingModel hostingModel, BlazorVersion blazorVersion)
    {
        var page = await _TestContext.GetPageAsync();
        var host = await _TestContext.StartHostAsync(hostingModel, blazorVersion);

        var fileName = $"{Guid.NewGuid()}.bin";

        var contentsSize = 4 * 1024; // 4KB
        var contentsBytes = new byte[contentsSize];
        Random.Shared.NextBytes(contentsBytes);

        await page.GotoAsync(host.GetUrl());
        await page.FillAsync("#file-name", fileName);
        await page.FillAsync("#content-type", "application/octet-stream");
        await page.FillAsync("#contents-base64", Convert.ToBase64String(contentsBytes));

        var downloadedItem = await page.RunAndWaitForDownloadAsync(() => page.ClickAsync("#download-button"));
        var path = await downloadedItem.PathAsync();
        var downloadedBytes = await File.ReadAllBytesAsync(path ?? throw new Exception("downloadedItem.PathAsync() returned null."));
        await downloadedItem.DeleteAsync();

        downloadedItem.SuggestedFilename.Is(fileName);
        downloadedBytes.SequenceEqual(contentsBytes);
    }

    [OneTimeTearDown]
    public async Task Cleanup()
    {
        await _TestContext.DisposeAsync();
    }
}