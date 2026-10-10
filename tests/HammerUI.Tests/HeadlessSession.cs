using Avalonia;
using Avalonia.Headless;
using HammerUI.Theming;

// Every test runs on the one headless UI thread, where a test's RunJobs would also run the jobs of tests in other classes.
[assembly: Xunit.v3.Parallelization(Mode = Xunit.Sdk.ParallelMode.None)]

namespace HammerUI.Tests;

/// <summary>The one headless Avalonia session of the test project; Avalonia starts only once per process.</summary>
internal static class HeadlessSession
{
    private static readonly Lazy<HeadlessUnitTestSession> Session = new(() => HeadlessUnitTestSession.StartNew(typeof(TestApp)));

    public static HeadlessUnitTestSession Value => Session.Value;

    private sealed class TestApp : Application
    {
        public override void Initialize() => Styles.Add(new ToolkitTheme());

        public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<TestApp>().UseHeadless(new AvaloniaHeadlessPlatformOptions());
    }
}
