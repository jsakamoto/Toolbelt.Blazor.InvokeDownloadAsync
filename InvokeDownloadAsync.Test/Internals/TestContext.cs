using Microsoft.Extensions.Configuration;
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

        {new SampleSiteKey(Wasm,   NET70),  new SampleSite(5019, "Wasm",   "net7.0")},
        {new SampleSiteKey(Server, NET70),  new SampleSite(5022, "Server", "net7.0")},
    };

    private IPlaywright? _Playwright = null;

    private IBrowser? _Browser = null;

    private IPage? _Page = null;

    private class TestOptions
    {
        public string Browser { get; set; } = "";

        public bool Headless { get; set; } = true;

        public bool SkipInstallBrowser { get; set; } = false;
    }

    private readonly TestOptions _Options = new();

    public TestContext()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables(prefix: "DOTNET_")
            .AddTestParameters()
            .Build();
        configuration.Bind(this._Options);

        if (!this._Options.SkipInstallBrowser)
        {
            Microsoft.Playwright.Program.Main(new[] { "install" });
        }
    }

    public async ValueTask<IPage> GetPageAsync()
    {
        this._Playwright ??= await Playwright.CreateAsync();
        this._Browser ??= await this.LaunchBrowserAsync(this._Playwright);
        this._Page ??= await this._Browser.NewPageAsync();
        return this._Page;
    }

    private Task<IBrowser> LaunchBrowserAsync(IPlaywright playwright)
    {
        var browserType = this._Options.Browser.ToLower() switch
        {
            "firefox" => playwright.Firefox,
            "webkit" => playwright.Webkit,
            _ => playwright.Chromium
        };

        var channel = this._Options.Browser.ToLower() switch
        {
            "firefox" or "webkit" => "",
            _ => this._Options.Browser.ToLower()
        };

        return browserType.LaunchAsync(new()
        {
            Channel = channel,
            Headless = this._Options.Headless,
        });
    }

    public ValueTask<SampleSite> StartHostAsync(HostingModel hostingModel, BlazorVersion blazorVersion)
    {
        return SampleSites[new SampleSiteKey(hostingModel, blazorVersion)].StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (this._Browser != null) await this._Browser.DisposeAsync();
        if (this._Playwright != null) this._Playwright.Dispose();
        Parallel.ForEach(SampleSites.Values, sampleSite => sampleSite.Stop());
    }
}
