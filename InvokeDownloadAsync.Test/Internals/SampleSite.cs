using Toolbelt.Diagnostics;
using static Toolbelt.Diagnostics.XProcess;

namespace Toolbelt.Blazor.InvokeDownloadAsync.Test.Internals;

public class SampleSite
{
    private readonly int ListenPort;

    private readonly string ProjectSubFolder;

    private readonly string TargetFramework;

    private XProcess? dotnetCLI;

    public SampleSite(int listenPort, string projectSubFolder, string targetFramework)
    {
        this.ListenPort = listenPort;
        this.ProjectSubFolder = projectSubFolder;
        this.TargetFramework = targetFramework;
    }

    public string GetUrl() => $"https://localhost:{this.ListenPort}";

    public async ValueTask<SampleSite> StartAsync()
    {
        if (this.dotnetCLI != null) return this;

        var solutionDir = FileIO.FindContainerDirToAncestor("Toolbelt.Blazor.InvokeDownloadAsync.sln");
        var sampleSiteDir = Path.Combine(solutionDir, "SampleSites");
        var projDir = Path.Combine(sampleSiteDir, this.ProjectSubFolder);
        this.dotnetCLI = Start("dotnet", $"run --urls {this.GetUrl()} -f {this.TargetFramework}", projDir);

        var success = await this.dotnetCLI.WaitForOutputAsync(output => output.Contains(this.GetUrl()), millsecondsTimeout: 15000);
        if (!success)
        {
            try { this.dotnetCLI.Dispose(); } catch { }
            var output = this.dotnetCLI.Output;
            this.dotnetCLI = null;
            throw new TimeoutException($"\"dotnet run\" did not respond \"Now listening on: {this.GetUrl()}\".\r\n" + output);
        }

        Thread.Sleep(200);
        return this;
    }

    public void Stop()
    {
        this.dotnetCLI?.Dispose();
    }
}
