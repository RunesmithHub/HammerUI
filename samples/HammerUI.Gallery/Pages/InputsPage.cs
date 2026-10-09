using Avalonia.Controls;
using Avalonia.Layout;
using HammerUI.Controls;
using static HammerUI.Gallery.Pages.Layout;
using AvaloniaColor = Avalonia.Media.Color;

namespace HammerUI.Gallery.Pages;

/// <summary>Text, choice and toggle inputs as the theme styles them.</summary>
internal static class InputsPage
{
    private static readonly string[] Sizes = ["Small", "Medium", "Large"];

    public static Control Create() => Page(
        Card("Text", Pairs(
            ("Text box", new TextBox { Text = "Hello", PlaceholderText = "Name" }),
            ("Placeholder", new TextBox { PlaceholderText = "Type to search" }),
            ("Search box", new SearchBox { PlaceholderText = "Filter files", Text = "readme" }),
            ("Multi-line", new TextBox { Text = "Lines wrap\nand grow the box.", AcceptsReturn = true, TextWrapping = Avalonia.Media.TextWrapping.Wrap, MinHeight = 64 }))),
        Card("Choices", Pairs(
            ("Combo box", new ComboBox { ItemsSource = Sizes, SelectedIndex = 1, HorizontalAlignment = HorizontalAlignment.Stretch }),
            ("Check box", new CheckBox { Content = "Show hidden files", IsChecked = true }),
            ("Toggle switch", new ToggleSwitch { IsChecked = true, OnContent = "Word wrap", OffContent = "Word wrap" }),
            ("Radio buttons", Row(new RadioButton { Content = "Spaces", IsChecked = true, GroupName = "indent" }, new RadioButton { Content = "Tabs", GroupName = "indent" })),
            ("Slider", new Slider { Minimum = 0, Maximum = 100, Value = 64 }),
            ("Color", new ColorPickerButton { Color = AvaloniaColor.Parse("#F5A35C"), HorizontalAlignment = HorizontalAlignment.Left, Width = 140 }))));
}
