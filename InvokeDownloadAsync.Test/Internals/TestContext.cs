using Microsoft.Playwright;
using static Toolbelt.Blazor.InvokeDownloadAsync.Test.Internals.BlazorVersion;
using static Toolbelt.Blazor.InvokeDownloadAsync.Test.Internals.HostingModel;

namespace Toolbelt.Blazor.InvokeDownloadAsync.Test.Internals;

public class TestContext : IAsyncDisposable
{
    public static readonly IReadOnlyDictionary<SampleSiteKey, SampleSite> SampleSites = new Dictionary<SampleSiteKey, SampleSite> {
        {new SampleSiteKey(Wasm,   NET50),  new SampleSite(5014, "Wasm",   "net5.0")},
        {new SampleSiteKey(Server, NET50),  new SampleSite(5017, "Server", "net5.0")},

        {new SampleSiteKey(Wasm,   NET60),  new SampleSite(5018, "Wasm",   "net6.0")},
        {new SampleSiteKey(Server, NET60),  new SampleSite(5021, "Server", "net6.0")},
    };

    private IPlaywright? _Playwright = null;

    private IBrowser? _Browser = null;

    private IPage? _Page = null;

    public async ValueTask<IPage> GetPageAsync()
    {
        if (this._Playwright == null) this._Playwright = await Playwright.CreateAsync();
        if (this._Browser == null)
        {
            this._Browser = await this._Playwright.Chromium.LaunchAsync(new()
            {
                Channel = "chrome",
                Headless = false
            });
        }
        if (this._Page == null) this._Page = await this._Browser.NewPageAsync();

        return this._Page;
    }

    public TestContext()
    {
    }

    public ValueTask<SampleSite> StartHostAsync(HostingModel hostingModel, BlazorVersion blazorVersion)
    {
        return SampleSites[new SampleSiteKey(hostingModel, blazorVersion)].StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        Parallel.ForEach(SampleSites.Values, sampleSite => sampleSite.Stop());
        if (this._Browser != null) await this._Browser.DisposeAsync();
        if (this._Playwright != null) this._Playwright.Dispose();
    }
}
