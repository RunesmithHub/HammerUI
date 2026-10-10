using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Layout;
using Avalonia.VisualTree;

namespace HammerUI.Controls;

/// <summary>Stacks the items of a menu and asks, from its first measure, for the width they take once their icon, key gesture and chevron
/// columns line up.</summary>
/// <remarks>The columns of a menu item share their widths through shared size groups, which Avalonia settles only a layout pass later. A
/// context menu opened at the pointer keeps the size of its first pass, so without this its widest rows would be cut off.</remarks>
internal sealed class MenuItemsPanel : StackPanel
{
    private static readonly string[] SharedParts = ["PART_ToggleIconPresenter", "PART_IconPresenter", "PART_InputGestureText", "PART_ChevronPath"];

    protected override Size MeasureOverride(Size availableSize)
    {
        var size = base.MeasureOverride(availableSize);
        if (Orientation != Orientation.Vertical)
            return size;

        var shared = new double[SharedParts.Length];
        var own = 0.0;
        foreach (var child in Children)
        {
            if (child is not MenuItem { IsVisible: true } item)
                continue;

            ContentPresenter? header = null;
            foreach (var part in item.GetVisualDescendants().OfType<Control>())
            {
                if (part.Name == "PART_HeaderPresenter" && part is ContentPresenter presenter)
                    header = presenter;
                else if (part.Name is { } name && Array.IndexOf(SharedParts, name) is >= 0 and var index)
                    shared[index] = Math.Max(shared[index], part.DesiredSize.Width);
            }

            if (header?.GetVisualParent() is Grid grid)
                own = Math.Max(own, item.DesiredSize.Width - grid.DesiredSize.Width + header.DesiredSize.Width);
        }

        var width = Math.Min(own + shared.Sum(), availableSize.Width);
        return width > size.Width ? size.WithWidth(width) : size;
    }
}
