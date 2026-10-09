using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace HammerUI.Docking;

/// <summary>The tab of one panel in a <see cref="DockGroupView"/>.</summary>
[TemplatePart(CloseButtonPart, typeof(Button))]
[PseudoClasses(":active", ":group-focused", ":closable", ":dragging", ":compact", ":modified", ":icon-brush")]
public class DockTab : TemplatedControl
{
    private const string CloseButtonPart = "PART_CloseButton";

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<DockTab, string?>(nameof(Title));

    public static readonly StyledProperty<Geometry?> IconProperty =
        AvaloniaProperty.Register<DockTab, Geometry?>(nameof(Icon));

    public static readonly StyledProperty<string?> DescriptionProperty =
        AvaloniaProperty.Register<DockTab, string?>(nameof(Description));

    public static readonly StyledProperty<IBrush?> IconBrushProperty =
        AvaloniaProperty.Register<DockTab, IBrush?>(nameof(IconBrush));

    public static readonly StyledProperty<bool> IsIconFilledProperty =
        AvaloniaProperty.Register<DockTab, bool>(nameof(IsIconFilled));

    public static readonly DirectProperty<DockTab, IBrush?> IconForegroundProperty =
        AvaloniaProperty.RegisterDirect<DockTab, IBrush?>(nameof(IconForeground), tab => tab.IconForeground);

    private Button? _closeButton;
    private IBrush? _iconForeground;

    internal DockTab(IDockPanel panel)
    {
        Panel = panel;
        Refresh();
    }

    /// <summary>Raised when the close button is clicked.</summary>
    public event EventHandler? CloseRequested;

    public IDockPanel Panel { get; }

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public Geometry? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>Gets or sets the icon's own color, or null to draw it in <see cref="TemplatedControl.Foreground"/>.</summary>
    public IBrush? IconBrush
    {
        get => GetValue(IconBrushProperty);
        set => SetValue(IconBrushProperty, value);
    }

    public bool IsIconFilled
    {
        get => GetValue(IsIconFilledProperty);
        set => SetValue(IsIconFilledProperty, value);
    }

    /// <summary>Gets the brush the icon is drawn with: <see cref="IconBrush"/>, or else the tab's foreground.</summary>
    public IBrush? IconForeground
    {
        get => _iconForeground;
        private set => SetAndRaise(IconForegroundProperty, ref _iconForeground, value);
    }

    /// <summary>Gets or sets the muted text after the title.</summary>
    public string? Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    internal bool IsActive
    {
        get => PseudoClasses.Contains(":active");
        set => PseudoClasses.Set(":active", value);
    }

    internal bool IsGroupFocused
    {
        set => PseudoClasses.Set(":group-focused", value);
    }

    internal bool IsDragging
    {
        set => PseudoClasses.Set(":dragging", value);
    }

    internal bool IsOverflowed { get; set; }

    /// <summary>Whether only the icon shows, because the strip is too narrow for every title.</summary>
    internal bool IsCompact
    {
        get => PseudoClasses.Contains(":compact");
        set => PseudoClasses.Set(":compact", value);
    }

    /// <summary>The width with the title, measured the last time the tab was not compact.</summary>
    internal double FullWidth { get; private set; }

    protected override Size MeasureOverride(Size availableSize)
    {
        var size = base.MeasureOverride(availableSize);
        if (!IsCompact && double.IsPositiveInfinity(availableSize.Width))
            FullWidth = size.Width;
        return size;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == TitleProperty || change.Property == DescriptionProperty)
            IsCompact = false;
        if (change.Property == IconBrushProperty || change.Property == ForegroundProperty)
            IconForeground = IconBrush ?? Foreground;
        if (change.Property == IconBrushProperty)
            PseudoClasses.Set(":icon-brush", IconBrush is not null);
    }

    internal Button? CloseButton => _closeButton;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _closeButton?.Click -= OnCloseClick;
        _closeButton = e.NameScope.Find<Button>(CloseButtonPart);
        _closeButton?.Click += OnCloseClick;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (Panel is INotifyPropertyChanged notifying)
            notifying.PropertyChanged += OnPanelPropertyChanged;
        Refresh();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (Panel is INotifyPropertyChanged notifying)
            notifying.PropertyChanged -= OnPanelPropertyChanged;
    }

    private void OnPanelPropertyChanged(object? sender, PropertyChangedEventArgs e) => Refresh();

    private void Refresh()
    {
        Title = Panel.Title;
        Icon = Panel.Icon;
        IconBrush = Panel.IconBrush;
        IsIconFilled = Panel.IsIconFilled;
        Description = Panel.Description;
        PseudoClasses.Set(":closable", Panel.CanClose);
        PseudoClasses.Set(":modified", Panel.IsModified);
        ToolTip.SetTip(this, Panel.ToolTip ?? Panel.Title);
    }

    private void OnCloseClick(object? sender, RoutedEventArgs e)
    {
        e.Handled = true;
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}
