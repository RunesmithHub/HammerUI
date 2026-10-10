# HammerUI

HammerUI is a modern, themeable and extensible UI library for [Avalonia](https://avaloniaui.net) on .NET 10. It gives an application a
complete look in light and dark, with an accent color the user can change at runtime, and the controls desktop tools need: docking with
tabs that drag between groups and out into their own windows, dialogs, toasts, property rows, number fields that scrub, color pickers,
curve and gradient editors, and a set of line icons.

## Features

- **Themes.** Light and dark palettes that follow the system or the user's choice, with any accent color, switched without a restart.
- **Docking.** Splits of tab groups whose tabs drag to any edge, into another group or out of the window; floating windows dock back;
  layouts save to JSON and restore, and named presets switch between them.
- **Controls.** Search boxes, segmented controls, badges, shortcut badges, property groups and rows, number and vector fields with
  expressions and scrubbing, path fields, color, curve and gradient editors, empty states, toasts and dialogs.
- **Icons.** Line icons on a 24×24 grid, found by name, with aliases; applications register their own and replace built-in ones.

## Getting started

HammerUI is published to the Runesmith hub feed, a public NuGet feed that needs no account or token. Add it as a source once:

```bash
dotnet nuget add source https://nuget.runesmith.dev/index.json --name runesmith-hub
```

[Getting started](website/content/guide/getting-started.mdx) shows a `nuget.config` that maps HammerUI to the feed. Then add the package,
and the theme to your application's styles:

```xml
<PackageReference Include="HammerUI" Version="0.1.1" />
```

```xml
<Application xmlns="https://github.com/avaloniaui"
             xmlns:ui="https://github.com/runesmithhub/hammerui"
             RequestedThemeVariant="Dark">
  <Application.Styles>
    <ui:ToolkitTheme />
  </Application.Styles>
</Application>
```

Every control is in the `https://github.com/runesmithhub/hammerui` XAML namespace. `ThemeManager` switches between light, dark and the
system theme and changes the accent color at runtime.

## Documentation

The documentation is in [`website/`](website/README.md): getting started, theming, every control, icons and docking. To read it locally:

```bash
cd website
npm install
npm start
```

The gallery shows every control in both themes:

```bash
dotnet run --project samples/HammerUI.Gallery
```

## Building

```bash
dotnet build HammerUI.slnx
dotnet test --solution HammerUI.slnx -- --ignore-exit-code 8
```

## Contributing

Contributions are welcome. [CONTRIBUTING.md](CONTRIBUTING.md) explains how to propose a change, and [SECURITY.md](SECURITY.md) how to report
a vulnerability privately.

## License

HammerUI is licensed under the [Apache License 2.0](LICENSE); see also [NOTICE](NOTICE). The software it uses keeps its own licenses:
[THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md) lists them.
