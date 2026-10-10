using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using HammerUI.Controls;

namespace HammerUI.Tests.Controls;

public sealed class MenuTests
{
    [Fact]
    public Task AMenuAsksForTheWidthOfItsLinedUpColumnsFromItsFirstMeasure() => HeadlessSession.Value.Dispatch(() =>
    {
        var menu = new ContextMenu
        {
            Items =
            {
                new MenuItem { Header = "Cut", Icon = new SymbolIcon { Data = Icons.Copy, Size = 14 }, InputGesture = KeyGesture.Parse("Ctrl+Shift+Alt+X") },
                new MenuItem { Header = "Rename Symbol", InputGesture = KeyGesture.Parse("F2") },
            },
        };
        var host = new Border();
        var window = new Window { Width = 800, Height = 600, Content = host };
        window.Show();
        Dispatcher.UIThread.RunJobs();

        host.Child = menu;
        menu.Measure(Size.Infinity);
        var first = menu.DesiredSize.Width;
        for (var pass = 0; pass < 3; pass++)
        {
            window.UpdateLayout();
            Dispatcher.UIThread.RunJobs();
        }

        menu.InvalidateMeasure();
        menu.Measure(Size.Infinity);
        Assert.Equal(menu.DesiredSize.Width, first, 0.5);
        window.Close();
    }, CancellationToken.None);
}
