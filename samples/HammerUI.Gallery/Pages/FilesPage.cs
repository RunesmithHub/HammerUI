using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Media;
using HammerUI.Controls;

namespace HammerUI.Gallery.Pages;

/// <summary>A file tree and asset tiles, as a file browser shows them.</summary>
internal static class FilesPage
{
    public static Control Create()
    {
        var tree = new TreeView
        {
            ItemsSource = new[]
            {
                new Node("src", Icons.Folder,
                [
                    new Node("App.axaml", Icons.FileCode), new Node("Program.cs", Icons.FileCode),
                    new Node("Pages", Icons.Folder, [new Node("Home.cs", Icons.FileCode), new Node("Settings.cs", Icons.FileCode)])
                ]),
                new Node("assets", Icons.Folder, [new Node("logo.png", Icons.Image), new Node("theme.ogg", Icons.Music)]),
                new Node("README.md", Icons.File),
            },
            ItemTemplate = new FuncTreeDataTemplate<Node>((node, _) => new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 6,
                Children =
                {
                    new SymbolIcon { Data = node.Icon, Size = 14 },
                    new TextBlock { Text = node.Name, VerticalAlignment = VerticalAlignment.Center }
                }
            }, node => node.Children ?? [])
        };

        var tiles = new WrapPanel { ItemSpacing = 8, LineSpacing = 8, Margin = new Thickness(12) };
        tiles.Children.Add(new AssetTile { Icon = Icons.Image, Title = "logo.png", TypeLabel = "PNG" });
        tiles.Children.Add(new AssetTile { Icon = Icons.Music, Title = "theme.ogg", TypeLabel = "OGG", IsSelected = true });
        tiles.Children.Add(new AssetTile { Icon = Icons.FileCode, Title = "Program.cs", TypeLabel = "C#" });

        var split = new Grid { RowDefinitions = new RowDefinitions("*,Auto") };
        split.Children.Add(tree);
        Grid.SetRow(tiles, 1);
        split.Children.Add(tiles);
        return split;
    }

    private sealed record Node(string Name, Geometry Icon, IReadOnlyList<Node>? Children = null);
}
