using Avalonia;
using Avalonia.Controls;
using Avalonia.Input.Platform;
using Avalonia.Layout;
using Avalonia.Media;
using HammerUI.Controls;

namespace HammerUI.Gallery.Pages;

/// <summary>Every icon with its lookup name; the search box filters them and a click copies the name.</summary>
internal static class IconsPage
{
    public static Control Create(IToastService toasts)
    {
        var search = new SearchBox { PlaceholderText = "Search icons", Margin = new Thickness(12, 12, 12, 8) };
        var wrap = new WrapPanel { Margin = new Thickness(12, 0, 12, 12), ItemSpacing = 6, LineSpacing = 6 };
        var tiles = Icons.Names.Select(name => (Name: name, Tile: Tile(name, toasts))).ToList();
        foreach (var (_, tile) in tiles)
            wrap.Children.Add(tile);

        search.PropertyChanged += (_, e) =>
        {
            if (e.Property != SearchBox.TextProperty)
                return;
            var text = search.Text?.Trim() ?? "";
            foreach (var (name, tile) in tiles)
                tile.IsVisible = name.Contains(text, StringComparison.OrdinalIgnoreCase);
        };

        var root = new DockPanel();
        DockPanel.SetDock(search, Dock.Top);
        root.Children.Add(search);
        root.Children.Add(new ScrollViewer { Content = wrap });
        return root;
    }

    private static Button Tile(string name, IToastService toasts)
    {
        var tile = new Button
        {
            Classes = { "subtle" },
            Width = 96,
            Height = 64,
            Padding = new Thickness(4),
            Content = new StackPanel
            {
                Spacing = 6,
                VerticalAlignment = VerticalAlignment.Center,
                Children =
                {
                    new SymbolIcon { Data = Icons.Find(name), Size = 20, HorizontalAlignment = HorizontalAlignment.Center },
                    new TextBlock { Text = name, FontSize = 10, Classes = { "muted" }, HorizontalAlignment = HorizontalAlignment.Center, TextTrimming = TextTrimming.CharacterEllipsis }
                }
            }
        };
        ToolTip.SetTip(tile, $"Icons.Find(\"{name}\")");
        tile.Click += async (_, _) =>
        {
            if (TopLevel.GetTopLevel(tile)?.Clipboard is { } clipboard)
            {
                await clipboard.SetTextAsync(name);
                toasts.Show("Copied", name, ToastKind.Success, TimeSpan.FromSeconds(2));
            }
        };
        return tile;
    }
}
