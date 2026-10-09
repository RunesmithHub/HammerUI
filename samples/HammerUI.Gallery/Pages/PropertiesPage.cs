using System.Numerics;
using Avalonia.Controls;
using Avalonia.Layout;
using HammerUI.Controls;
using AvaloniaColor = Avalonia.Media.Color;

namespace HammerUI.Gallery.Pages;

/// <summary>A property panel: collapsible groups of rows whose labels share one resizable column.</summary>
internal static class PropertiesPage
{
    public static Control Create()
    {
        var window = new PropertyGroup
        {
            Header = "Window",
            Icon = Icons.Monitor,
            IsExpanded = true,
            Content = Rows(
                new PropertyRow { Label = "Title", Description = "Shown in the title bar and the task switcher", Content = new TextBox { Text = "Untitled" } },
                new PropertyRow { Label = "Size", Content = new Vector2Field { Value = new Vector2(1280, 720) } },
                new PropertyRow { Label = "Opacity", IsModified = true, Content = new NumberField { Value = 0.9, Step = 0.05, FormatString = "0.00", Minimum = 0, Maximum = 1 } },
                new PropertyRow { Label = "Resizable", Content = new CheckBox { IsChecked = true } },
                new PropertyRow { Label = "Accent", Content = new ColorPickerButton { Color = AvaloniaColor.Parse("#6366F1"), HorizontalAlignment = HorizontalAlignment.Left } })
        };
        var animation = new PropertyGroup
        {
            Header = "Animation",
            Icon = Icons.Activity,
            IsExpanded = true,
            Content = Rows(
                new PropertyRow { Label = "Duration", Content = new NumberField { Value = 250, Suffix = "ms", IsInteger = true, Minimum = 0 } },
                new PropertyRow { Label = "Easing", Content = new CurvePreview { Curve = CurvePresets.EaseInOut } },
                new PropertyRow { Label = "Colors", Content = new GradientPreview { Gradient = FieldsPage.Sunset } })
        };
        var stack = new StackPanel();
        stack.Children.Add(window);
        stack.Children.Add(animation);
        return new ScrollViewer { Content = stack };
    }

    private static StackPanel Rows(params Control[] rows)
    {
        var stack = new StackPanel();
        stack.Children.AddRange(rows);
        return stack;
    }
}
