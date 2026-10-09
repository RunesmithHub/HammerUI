using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using HammerUI.Controls;
using HammerUI.Docking;

namespace HammerUI.Tests.Docking;

public sealed class DockTabTests
{
    [Fact]
    public Task ATabDrawsItsPanelsIconInThePanelsOwnColorAlsoWhileActive() => HeadlessSession.Value.Dispatch(() =>
    {
        var colored = new ColoredPanel("a", Brushes.Orange);
        var plain = new ColoredPanel("b", null);
        var host = new DockHost
        {
            Layout = new DockLayout(new DockGroup("main", "a", "b")),
            ContentProvider = new DockContentProvider { colored, plain },
        };
        var window = new Window { Width = 600, Height = 400, Content = host };
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var icons = host.GetVisualDescendants().OfType<DockTab>().ToDictionary(t => t.Panel.Id, t => t.GetVisualDescendants().OfType<SymbolIcon>().First(i => i.Name == "PART_Icon"));

        Assert.Same(Brushes.Orange, icons["a"].Foreground);
        Assert.True(icons["a"].IsFilled);
        Assert.NotSame(Brushes.Orange, icons["b"].Foreground);
        Assert.False(icons["b"].IsFilled);
        window.Close();
    }, CancellationToken.None);

    private sealed class ColoredPanel(string id, IBrush? brush) : IDockPanel
    {
        public string Id => id;

        public string Title => id;

        public Geometry? Icon => Icons.File;

        public IBrush? IconBrush => brush;

        public bool IsIconFilled => brush is not null;

        public bool CanClose => true;

        public Control Content { get; } = new Border();
    }
}
