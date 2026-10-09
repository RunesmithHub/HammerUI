using Avalonia;
using HammerUI.Gallery;

var builder = AppBuilder.Configure<App>().UsePlatformDetect().WithInterFont().LogToTrace();
#if DEBUG
builder = builder.WithDeveloperTools();
#endif
return builder.StartWithClassicDesktopLifetime(args);
