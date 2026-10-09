using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace HammerUI.Controls;

/// <summary>The button of one <see cref="ToolStripeItem"/> on a <see cref="ToolStripe"/>; it clicks, and drags to another place.</summary>
[PseudoClasses(":active", ":badged", ":dragging")]
public class ToolStripeButton : Button
{
    public static readonly StyledProperty<ToolStripeItem?> ItemProperty =
        AvaloniaProperty.Register<ToolStripeButton, ToolStripeItem?>(nameof(Item));

    public static readonly StyledProperty<Geometry?> IconProperty =
        AvaloniaProperty.Register<ToolStripeButton, Geometry?>(nameof(Icon));

    public static readonly StyledProperty<string?> BadgeProperty =
        AvaloniaProperty.Register<ToolStripeButton, string?>(nameof(Badge));

    private const double DragThreshold = 6;

    private Point? _pressedAt;
    private Drag? _drag;
    private bool _suppressClick;
    private KeyModifiers _modifiers;
    private bool _cancelled;

    /// <summary>Gets or sets the item the button shows.</summary>
    public ToolStripeItem? Item
    {
        get => GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    public Geometry? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string? Badge
    {
        get => GetValue(BadgeProperty);
        set => SetValue(BadgeProperty, value);
    }

    protected override Type StyleKeyOverride => typeof(ToolStripeButton);

    private ToolStripe? Stripe => this.FindAncestorOfType<ToolStripe>();

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == ItemProperty)
        {
            if (change.OldValue is ToolStripeItem old)
                old.PropertyChanged -= OnItemChanged;
            if (change.NewValue is ToolStripeItem item && this.IsAttachedToVisualTree())
                item.PropertyChanged += OnItemChanged;
            Refresh();
        }
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        Item?.PropertyChanged += OnItemChanged;
        Refresh();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        Item?.PropertyChanged -= OnItemChanged;
        EndDrag(drop: false);
    }

    protected override void OnClick()
    {
        if (_suppressClick)
            return;

        base.OnClick();
        if (Item is { } item)
            Stripe?.Invoke(item, _modifiers);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(this);
        if (point.Properties.IsLeftButtonPressed)
            _pressedAt = e.GetPosition(TopLevel.GetTopLevel(this));
        base.OnPointerPressed(e);
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        if (_pressedAt is not { } start || Item is null || TopLevel.GetTopLevel(this) is not { } top || !e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            return;

        var position = e.GetPosition(top);
        if (_drag is null)
        {
            if (Math.Abs(position.X - start.X) < DragThreshold && Math.Abs(position.Y - start.Y) < DragThreshold)
                return;

            _drag = new Drag(top, Item, () =>
            {
                _cancelled = true;
                EndDrag(drop: false);
            });
            PseudoClasses.Set(":dragging", true);
        }

        _drag.Update(position);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        var dragged = _drag is not null;
        _modifiers = e.KeyModifiers;
        EndDrag(drop: true);
        _suppressClick = dragged || _cancelled;
        _cancelled = false;
        try
        {
            base.OnPointerReleased(e);
        }
        finally
        {
            _suppressClick = false;
        }
    }

    protected override void OnPointerCaptureLost(PointerCaptureLostEventArgs e)
    {
        base.OnPointerCaptureLost(e);
        EndDrag(drop: false);
    }

    public ToolStripeButton()
    {
        var menu = new ContextMenu();
        menu.Opening += (_, e) =>
        {
            menu.Items.Clear();
            if (Item is { } item && Stripe?.ItemMenu is { } create)
            {
                foreach (var entry in create(item))
                    menu.Items.Add(entry);
            }

            e.Cancel = menu.Items.Count == 0;
        };
        ContextMenu = menu;
    }

    private void OnItemChanged(object? sender, PropertyChangedEventArgs e) => Refresh();

    private void Refresh()
    {
        var item = Item;
        Icon = item?.Icon;
        Badge = item?.Badge;
        PseudoClasses.Set(":active", item?.IsActive == true);
        PseudoClasses.Set(":badged", !string.IsNullOrEmpty(item?.Badge));
        ToolTip.SetTip(this, item?.ToolTip ?? item?.Title);
    }

    private void EndDrag(bool drop)
    {
        _pressedAt = null;
        if (_drag is null)
            return;

        var drag = _drag;
        _drag = null;
        PseudoClasses.Set(":dragging", false);
        drag.End(drop ? Stripe : null);
    }

    /// <summary>A button being dragged: finds the stripe under the pointer and shows where the button would land.</summary>
    private sealed class Drag
    {
        private readonly TopLevel _top;
        private readonly ToolStripeItem _item;
        private readonly Action _cancel;
        private readonly List<ToolStripe> _stripes;
        private (ToolStripe Stripe, ToolStripeGroup Group, int Index)? _target;

        public Drag(TopLevel top, ToolStripeItem item, Action cancel)
        {
            _top = top;
            _item = item;
            _cancel = cancel;
            _stripes = [.. top.GetVisualDescendants().OfType<ToolStripe>().Where(s => s.IsEffectivelyVisible)];
            top.AddHandler(KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);
        }

        public void Update(Point position)
        {
            _target = null;
            foreach (var stripe in _stripes)
            {
                var local = _top.TranslatePoint(position, stripe);
                if (local is { } point && new Rect(stripe.Bounds.Size).Inflate(new Thickness(24, 0)).Contains(point))
                {
                    var (group, index, lineY) = stripe.DropTarget(point, _item);
                    _target = (stripe, group, index);
                    stripe.ShowIndicator(lineY);
                }
                else
                {
                    stripe.ShowIndicator(null);
                }
            }
        }

        public void End(ToolStripe? source)
        {
            _top.RemoveHandler(KeyDownEvent, OnKeyDown);
            foreach (var stripe in _stripes)
                stripe.ShowIndicator(null);
            if (source is not null && _target is { } target)
                source.Move(_item, target.Stripe, target.Group, target.Index);
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape)
                return;

            e.Handled = true;
            _cancel();
        }
    }
}
