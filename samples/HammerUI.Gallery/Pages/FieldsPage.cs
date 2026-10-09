using System.Numerics;
using Avalonia.Controls;
using HammerUI.Controls;
using HammerUI.Values;
using static HammerUI.Gallery.Pages.Layout;

namespace HammerUI.Gallery.Pages;

/// <summary>Editor-grade fields: numbers that scrub and accept expressions, vectors, paths, and curve and gradient swatches.</summary>
internal static class FieldsPage
{
    public static Control Create() => Page(
        Card("Number fields", Pairs(
                ("Integer", new NumberField { Value = 36, Label = "W", Suffix = "px", IsInteger = true }),
                ("Fraction", new NumberField { Value = 0.75, Step = 0.01, FormatString = "0.00", Minimum = 0, Maximum = 1 }),
                ("Mixed", new NumberField { IsMixed = true, Label = "H" }),
                ("Disabled", new NumberField { Value = 90, Suffix = "°", IsEnabled = false }),
                ("Vector", new Vector2Field { Value = new Vector2(128, -64.5f) })),
            "Drag a field's label to scrub its value, or type an expression such as 12 * 4 + 2."),
        Card("Paths", Pairs(
            ("File", new PathField { PlaceholderText = "Choose a file", Mode = PathFieldMode.OpenFile }),
            ("Folder", new PathField { PlaceholderText = "Choose a folder", Mode = PathFieldMode.Folder }))),
        Card("Curves and gradients", Pairs(
                ("Curve", new CurvePreview { Curve = CurvePresets.EaseOut }),
                ("Gradient", new GradientPreview { Gradient = Sunset })),
            "Click a swatch to edit it in a flyout."));

    public static Gradient Sunset { get; } = new([
        new GradientStop(0, new Color(255, 205, 120)), new GradientStop(0.55f, new Color(240, 110, 90)), new GradientStop(1, new Color(90, 70, 160, 0))]);
}
