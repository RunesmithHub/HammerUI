using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Input;
using Avalonia.Media;

namespace HammerUI.Controls;

/// <summary>The group of a <see cref="ToolStripe"/> an item sits in: the upper one from the top edge, or the lower one from the bottom edge.</summary>
public enum ToolStripeGroup
{
    Top,
    Bottom,
}

/// <summary>A button of a <see cref="ToolStripe"/>: a panel's icon, its title, an optional badge and whether the panel shows.</summary>
/// <param name="id">The id the application knows the panel by.</param>
/// <param name="title">The panel's title, shown as the button's tooltip.</param>
/// <param name="icon">The panel's 24 × 24 line icon.</param>
public sealed class ToolStripeItem(string id, string title, Geometry? icon) : INotifyPropertyChanged
{
    private string _title = title;
    private Geometry? _icon = icon;
    private string? _toolTip;
    private string? _badge;
    private bool _isActive;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Id { get; } = id;

    public string Title
    {
        get => _title;
        set => Set(ref _title, value);
    }

    public Geometry? Icon
    {
        get => _icon;
        set => Set(ref _icon, value);
    }

    /// <summary>Gets or sets the tooltip, such as the title with the key that shows the panel; the title when null.</summary>
    public string? ToolTip
    {
        get => _toolTip;
        set => Set(ref _toolTip, value);
    }

    /// <summary>Gets or sets the short text drawn over the button's corner, such as a count, or null for none.</summary>
    public string? Badge
    {
        get => _badge;
        set => Set(ref _badge, value);
    }

    /// <summary>Gets or sets whether the item's panel shows; the button draws as pressed.</summary>
    public bool IsActive
    {
        get => _isActive;
        set => Set(ref _isActive, value);
    }

    private void Set<T>(ref T field, T value, [CallerMemberName] string name = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

/// <summary>Says which <see cref="ToolStripeItem"/> was clicked, and with which modifier keys.</summary>
public sealed class ToolStripeItemEventArgs(ToolStripeItem item, KeyModifiers modifiers) : EventArgs
{
    public ToolStripeItem Item { get; } = item;

    public KeyModifiers Modifiers { get; } = modifiers;
}

/// <summary>Says where a dragged <see cref="ToolStripeItem"/> was dropped.</summary>
/// <param name="item">The dragged item.</param>
/// <param name="target">The stripe it was dropped on, which may be the one it came from.</param>
/// <param name="group">The group of <paramref name="target"/> it was dropped in.</param>
/// <param name="index">Its position in that group, counted without the item itself.</param>
public sealed class ToolStripeItemMovedEventArgs(ToolStripeItem item, ToolStripe target, ToolStripeGroup group, int index) : EventArgs
{
    public ToolStripeItem Item { get; } = item;

    public ToolStripe Target { get; } = target;

    public ToolStripeGroup Group { get; } = group;

    public int Index { get; } = index;
}
