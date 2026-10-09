using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;
using HammerUI.Controls;
using HammerUI.Docking;
using HammerUI.Gallery.Pages;
using HammerUI.Services;
using HammerUI.Theming;

namespace HammerUI.Gallery;

/// <summary>The gallery's window: an app bar with the theme and accent controls, and a dock workspace with one panel per kind of control.</summary>
public partial class MainWindow : Window
{
    private readonly IThemeManager _theme;
    private readonly DockContentProvider _panels;

    /// <summary>Creates the window with a theme manager of its own, as the XAML previewer does.</summary>
    public MainWindow()
        : this(new ThemeManager(Avalonia.Application.Current!))
    {
    }

    public MainWindow(IThemeManager theme)
    {
        _theme = theme;
        InitializeComponent();

        var host = new WindowHost();
        host.Attach(this, Dialogs, Toasts);
        var toasts = new WindowToastService(host);
        var dialogs = new DialogService(host);

        _panels = new DockContentProvider
        {
            new DockablePanel("buttons", "Buttons", ButtonsPage.Create) { Icon = Icons.MousePointer, CanClose = false },
            new DockablePanel("inputs", "Inputs", InputsPage.Create) { Icon = Icons.Type },
            new DockablePanel("fields", "Fields", FieldsPage.Create) { Icon = Icons.Sliders },
            new DockablePanel("properties", "Properties", PropertiesPage.Create) { Icon = Icons.ListTree },
            new DockablePanel("editors", "Curves and gradients", EditorsPage.Create) { Icon = Icons.Spline },
            new DockablePanel("feedback", "Feedback", () => FeedbackPage.Create(toasts, dialogs)) { Icon = Icons.CheckCircle },
            new DockablePanel("icons", "Icons", () => IconsPage.Create(toasts)) { Icon = Icons.Palette },
            new DockablePanel("files", "Files", FilesPage.Create) { Icon = Icons.FolderOpen },
        };

        Workspace.ContentProvider = _panels;
        Workspace.Layout = CreateLayout();
        Workspace.WindowOpened += (_, e) => e.Window.Icon = Icon;

        AccentPicker.Color = _theme.Accent;
        AccentPicker.PropertyChanged += (_, e) =>
        {
            if (e.Property == ColorPickerButton.ColorProperty)
                _theme.Accent = AccentPicker.Color;
        };
        ThemeToggle.Click += OnToggleTheme;
        ResetLayout.Click += (_, _) => Workspace.Layout = CreateLayout();
        _theme.Changed += (_, _) => UpdateThemeIcon();
        UpdateThemeIcon();
    }

    private static DockLayout CreateLayout()
    {
        var layout = new DockLayout(
            new DockSplit("root", DockOrientation.Horizontal,
                new DockGroup("left", "files", "icons") { Size = 0.22 },
                new DockSplit("middle", DockOrientation.Vertical,
                    new DockGroup("center", "buttons", "inputs", "fields") { Size = 0.62 },
                    new DockGroup("bottom", "feedback", "editors") { Size = 0.38 })
                { Size = 0.52 },
                new DockGroup("right", "properties") { Size = 0.26 }));
        layout.ActivatePanel("buttons");
        return layout;
    }

    private void OnToggleTheme(object? sender, RoutedEventArgs e) =>
        _theme.Mode = _theme.ActualVariant == ThemeVariant.Dark ? ThemeMode.Light : ThemeMode.Dark;

    private void UpdateThemeIcon() => ThemeIcon.Data = _theme.ActualVariant == ThemeVariant.Dark ? Icons.Sun : Icons.Moon;
}
