using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace HammerUI.Controls;

/// <summary>A narrow column of icon buttons at an edge of a window, one per panel, in an upper and a lower group. Clicking a button raises
/// <see cref="ItemInvoked"/>; buttons can be dragged within the stripe and onto other stripes of the same window, which raises
/// <see cref="ItemMoved"/>. The application owns the items and moves them.</summary>
[TemplatePart(TopPart, typeof(ItemsControl))]
[TemplatePart(BottomPart, typeof(ItemsControl))]
[TemplatePart(IndicatorPart, typeof(Control))]
public class ToolStripe : TemplatedControl
{
    private const string TopPart = "PART_Top";
    private const string BottomPart = "PART_Bottom";
    private const string IndicatorPart = "PART_Indicator";

    public static readonly StyledProperty<IEnumerable?> TopItemsProperty =
        AvaloniaProperty.Register<ToolStripe, IEnumerable?>(nameof(TopItems));

    public static readonly StyledProperty<IEnumerable?> BottomItemsProperty =
        AvaloniaProperty.Register<ToolStripe, IEnumerable?>(nameof(BottomItems));

    private ItemsControl? _top;
    private ItemsControl? _bottom;
    private Control? _indicator;

    /// <summary>Gets or sets the <see cref="ToolStripeItem"/>s of the upper group, usually an observable collection.</summary>
    public IEnumerable? TopItems
    {
        get => GetValue(TopItemsProperty);
        set => SetValue(TopItemsProperty, value);
    }

    /// <summary>Gets or sets the <see cref="ToolStripeItem"/>s of the lower group, usually an observable collection.</summary>
    public IEnumerable? BottomItems
    {
        get => GetValue(BottomItemsProperty);
        set => SetValue(BottomItemsProperty, value);
    }

    /// <summary>Gets or sets what creates the entries of a button's context menu each time it opens, such as items that move its panel
    /// elsewhere; no entries means no menu.</summary>
    public Func<ToolStripeItem, IEnumerable<Control>>? ItemMenu { get; set; }

    /// <summary>Raised when a button is clicked.</summary>
    public event EventHandler<ToolStripeItemEventArgs>? ItemInvoked;

    /// <summary>Raised on the stripe a button was dragged from, when it is dropped on a stripe.</summary>
    public event EventHandler<ToolStripeItemMovedEventArgs>? ItemMoved;

    /// <summary>Finds where an item dropped at <paramref name="y"/> goes among buttons whose centers are at <paramref name="centers"/>, from
    /// the top.</summary>
    internal static int InsertionIndex(IReadOnlyList<double> centers, double y) => centers.Count(center => center < y);

    /// <summary>Picks the group for a point at <paramref name="y"/>: the lower one below the middle of the gap between the groups.</summary>
    internal static ToolStripeGroup PickGroup(double topGroupBottom, double bottomGroupTop, double y) =>
        y >= (topGroupBottom + bottomGroupTop) / 2 ? ToolStripeGroup.Bottom : ToolStripeGroup.Top;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _top = e.NameScope.Find<ItemsControl>(TopPart);
        _bottom = e.NameScope.Find<ItemsControl>(BottomPart);
        _indicator = e.NameScope.Find<Control>(IndicatorPart);
    }

    internal void Invoke(ToolStripeItem item, KeyModifiers modifiers) => ItemInvoked?.Invoke(this, new ToolStripeItemEventArgs(item, modifiers));

    internal void Move(ToolStripeItem item, ToolStripe target, ToolStripeGroup group, int index) =>
        ItemMoved?.Invoke(this, new ToolStripeItemMovedEventArgs(item, target, group, index));

    /// <summary>Finds the group and position for <paramref name="dragged"/> at <paramref name="point"/>, in this stripe's coordinates, and
    /// where to draw the insertion line.</summary>
    internal (ToolStripeGroup Group, int Index, double LineY) DropTarget(Point point, ToolStripeItem dragged)
    {
        if (_top is null || _bottom is null)
            return (ToolStripeGroup.Top, 0, 0);

        var topBounds = BoundsIn(_top);
        var bottomBounds = BoundsIn(_bottom);
        var group = PickGroup(topBounds.Bottom, bottomBounds.Top, point.Y);
        var part = group == ToolStripeGroup.Top ? _top : _bottom;
        var buttons = part.GetRealizedContainers()
            .Select(c => c.FindDescendantOfType<ToolStripeButton>() ?? c as ToolStripeButton)
            .OfType<ToolStripeButton>()
            .Where(b => !ReferenceEquals(b.Item, dragged))
            .Select(BoundsIn)
            .OrderBy(b => b.Top)
            .ToList();
        var index = InsertionIndex([.. buttons.Select(b => b.Center.Y)], point.Y);
        var bounds = group == ToolStripeGroup.Top ? topBounds : bottomBounds;
        var lineY = buttons.Count == 0 ? (group == ToolStripeGroup.Top ? bounds.Top : bounds.Bottom)
            : index < buttons.Count ? buttons[index].Top - 2
            : buttons[^1].Bottom + 2;
        return (group, index, lineY);
    }

    internal void ShowIndicator(double? y)
    {
        if (_indicator is null)
            return;

        _indicator.IsVisible = y is not null;
        if (y is { } top)
            Canvas.SetTop(_indicator, top - _indicator.Height / 2);
    }

    private Rect BoundsIn(Visual visual) =>
        visual.TranslatePoint(default, this) is { } origin ? new Rect(origin, visual.Bounds.Size) : default;
}
