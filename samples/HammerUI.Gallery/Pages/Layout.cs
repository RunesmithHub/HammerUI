using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace HammerUI.Gallery.Pages;

/// <summary>Building blocks shared by the pages: scrolling page bodies, titled cards and rows of controls.</summary>
internal static class Layout
{
    public static Control Page(params Control[] cards)
    {
        var stack = new StackPanel { Spacing = 16, Margin = new Thickness(16) };
        stack.Children.AddRange(cards);
        return new ScrollViewer { Content = stack };
    }

    public static Border Card(string title, Control content, string? description = null)
    {
        var body = new StackPanel { Spacing = 12 };
        body.Children.Add(new TextBlock { Text = title.ToUpperInvariant(), Classes = { "section" } });
        if (description is not null)
            body.Children.Add(new TextBlock { Text = description, Classes = { "muted" }, TextWrapping = Avalonia.Media.TextWrapping.Wrap });
        body.Children.Add(content);
        return new Border { Classes = { "card" }, Padding = new Thickness(16, 14, 16, 16), Child = body };
    }

    public static WrapPanel Row(params Control[] children)
    {
        var row = new WrapPanel { ItemSpacing = 8, LineSpacing = 8 };
        foreach (var child in children)
        {
            child.VerticalAlignment = VerticalAlignment.Center;
            row.Children.Add(child);
        }

        return row;
    }

    public static Grid Pairs(params (string Label, Control Control)[] pairs)
    {
        var grid = new Grid { ColumnDefinitions = new ColumnDefinitions("120,*"), ColumnSpacing = 12, RowSpacing = 10 };
        for (var i = 0; i < pairs.Length; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            var label = new TextBlock { Text = pairs[i].Label, Classes = { "muted" }, VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(label, i);
            Grid.SetRow(pairs[i].Control, i);
            Grid.SetColumn(pairs[i].Control, 1);
            grid.Children.Add(label);
            grid.Children.Add(pairs[i].Control);
        }

        return grid;
    }
}
