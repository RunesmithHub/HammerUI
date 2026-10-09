using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using HammerUI.Controls;
using static HammerUI.Gallery.Pages.Layout;

namespace HammerUI.Gallery.Pages;

/// <summary>Buttons in every style, tool toggles, badges, segmented controls and shortcut badges.</summary>
internal static class ButtonsPage
{
    private static readonly string[] Ranges = ["Day", "Week", "Month"];

    public static Control Create() => Page(
        Card("Buttons", Row(
                new Button { Classes = { "accent" }, Content = "Save" },
                new Button { Content = "Cancel" },
                new Button { Classes = { "subtle" }, Content = "Reset" },
                new Button { Classes = { "danger" }, Content = "Delete" },
                new Button { Content = "Disabled", IsEnabled = false }),
            "Classes pick the role: accent for the main action, subtle for quiet ones and danger for destructive ones."),
        Card("Icon buttons and tools", Row(
            Icon(Icons.Undo, "Undo"), Icon(Icons.Redo, "Redo"), Icon(Icons.Copy, "Copy"), Icon(Icons.Trash, "Delete"),
            new Border { Classes = { "divider-v" }, Height = 22 },
            Tool(Icons.MousePointer, true), Tool(Icons.Move), Tool(Icons.RotateCw), Tool(Icons.Scale), Tool(Icons.SquareDashed))),
        Card("Badges", Row(
            new Badge { Content = "Default" }, new Badge { Classes = { "accent" }, Content = "New" },
            new Badge { Classes = { "success" }, Content = "Saved" }, new Badge { Classes = { "warning" }, Content = "3 warnings" },
            new Badge { Classes = { "danger" }, Content = "Error" })),
        Card("Segmented controls and shortcuts", Row(
            new SegmentedControl { ItemsSource = Ranges, SelectedIndex = 1 },
            new ShortcutBadge { Text = "Ctrl+K" }, new ShortcutBadge { Text = "Ctrl+Shift+P" }, new ShortcutBadge { Text = "F12" })));

    private static Button Icon(Geometry icon, string tip)
    {
        var button = new Button { Classes = { "icon" }, Content = new SymbolIcon { Data = icon } };
        ToolTip.SetTip(button, tip);
        return button;
    }

    private static ToggleButton Tool(Geometry icon, bool isChecked = false) =>
        new() { Classes = { "tool" }, IsChecked = isChecked, Content = new SymbolIcon { Data = icon, Size = 18 } };
}
