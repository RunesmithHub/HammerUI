using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Media;
using Avalonia.Threading;
using HammerUI.Controls;

namespace HammerUI.Tests.Controls;

public sealed class SymbolIconTests
{
    [Fact]
    public Task ATurningIconSpinsWithoutTakingOverItsTransform() => HeadlessSession.Value.Dispatch(() =>
    {
        var turned = new RotateTransform(90);
        var still = new SymbolIcon { Data = Icons.ChevronRight };
        var turning = new SymbolIcon { Data = Icons.RotateCw, Classes = { "spin" }, RenderTransform = turned };
        var window = new Window { Content = new StackPanel { Children = { still, turning } } };
        window.Show();
        var angle = AvaloniaPropertyRegistry.Instance.FindRegistered(turning, "SpinAngle")!;

        for (var i = 0; i < 10; i++)
        {
            AvaloniaHeadlessPlatform.ForceRenderTimerTick();
            Dispatcher.UIThread.RunJobs();
        }

        Assert.True(turning.IsSpinning);
        Assert.Same(turned, turning.RenderTransform);
        Assert.Null(still.RenderTransform);
        Assert.True((double)turning.GetValue(angle)! > 0, "The icon did not turn.");

        turning.Classes.Remove("spin");
        Assert.False(turning.IsSpinning);
        Assert.Same(turned, turning.RenderTransform);
        window.Close();
    }, CancellationToken.None);
}
