using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HammerUI.Theming;

namespace HammerUI.Gallery;

public sealed class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var theme = new ThemeManager(this);
            desktop.MainWindow = new MainWindow(theme);
            desktop.Exit += (_, _) => theme.Dispose();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
