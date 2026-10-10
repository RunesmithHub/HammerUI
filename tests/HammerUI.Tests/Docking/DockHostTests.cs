using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using HammerUI.Docking;

namespace HammerUI.Tests.Docking;

public sealed class DockHostTests
{
    [Fact]
    public Task APanelOpenedAgainUnderTheSameIdShowsItsNewContent() => HeadlessSession.Value.Dispatch(() =>
    {
        var provider = new Provider();
        var first = provider.Open("doc");
        var (host, window) = Show(provider, new DockGroup("main", "doc"));

        host.Layout!.ClosePanel("doc");
        var second = provider.Open("doc");
        host.Layout.MovePanel("doc", "main");
        Dispatcher.UIThread.RunJobs();

        Assert.Same(second.Content, host.GetContent("doc"));
        Assert.Contains(second.Content, host.GetVisualDescendants());
        Assert.DoesNotContain(first.Content, host.GetVisualDescendants());
        window.Close();
    }, CancellationToken.None);

    [Fact]
    public Task APanelTheProviderReplacesWhileOpenShowsTheNewContentOnTheNextChange() => HeadlessSession.Value.Dispatch(() =>
    {
        var provider = new Provider();
        provider.Open("a");
        provider.Open("b");
        var (host, window) = Show(provider, new DockGroup("main", "a", "b"));

        var replaced = provider.Open("a");
        host.Layout!.ClosePanel("b");
        Dispatcher.UIThread.RunJobs();

        Assert.Same(replaced.Content, host.GetContent("a"));
        Assert.Contains(replaced.Content, host.GetVisualDescendants());
        window.Close();
    }, CancellationToken.None);

    [Fact]
    public Task APanelKeepsItsContentWhileItMovesBetweenGroups() => HeadlessSession.Value.Dispatch(() =>
    {
        var provider = new Provider();
        var panel = provider.Open("a");
        provider.Open("b");
        var (host, window) = Show(provider, new DockSplit("root", DockOrientation.Horizontal, new DockGroup("left", "a"), new DockGroup("right", "b")));
        var content = host.GetContent("a");

        host.Layout!.MovePanel("a", "right");
        Dispatcher.UIThread.RunJobs();

        Assert.Same(content, host.GetContent("a"));
        Assert.Equal(1, panel.Created);
        window.Close();
    }, CancellationToken.None);

    private static (DockHost Host, Window Window) Show(Provider provider, DockNode root)
    {
        var host = new DockHost { Layout = new DockLayout(root), ContentProvider = provider };
        var window = new Window { Width = 600, Height = 400, Content = host };
        window.Show();
        Dispatcher.UIThread.RunJobs();
        return (host, window);
    }

    private sealed class Provider : IDockContentProvider
    {
        private readonly Dictionary<string, TestPanel> _panels = [];

        public TestPanel Open(string id) => _panels[id] = new TestPanel(id);

        public IDockPanel? GetPanel(string panelId) => _panels.GetValueOrDefault(panelId);
    }

    private sealed class TestPanel(string id) : IDockPanel
    {
        private Control? _content;

        public int Created { get; private set; }

        public string Id => id;

        public string Title => id;

        public Geometry? Icon => null;

        public bool CanClose => true;

        public Control Content => _content ??= Create();

        private Border Create()
        {
            Created++;
            return new Border();
        }
    }
}
