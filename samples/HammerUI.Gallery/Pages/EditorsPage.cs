using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using HammerUI.Controls;
using HammerUI.Values;

namespace HammerUI.Gallery.Pages;

/// <summary>The curve and gradient editors at full size.</summary>
internal static class EditorsPage
{
    public static Control Create()
    {
        var curve = new CurveEditor
        {
            Curve = new Curve([new CurveKey(0, 0.1f, 0, 0), new CurveKey(0.3f, 1, 0.6f, 0.6f), new CurveKey(0.75f, 0.55f, -1.2f, -1.2f),
                new CurveKey(1, 0, 0, 0, CurveInterpolation.Linear)])
        };
        var gradient = new GradientEditor { Gradient = FieldsPage.Sunset };
        var row = new WrapPanel { ItemSpacing = 20, LineSpacing = 20, Margin = new Thickness(16) };
        row.Children.Add(Framed(curve));
        row.Children.Add(Framed(gradient));
        return new ScrollViewer { Content = row };
    }

    private static Border Framed(Control content) => new()
    {
        Classes = { "card" },
        Padding = new Thickness(12),
        VerticalAlignment = VerticalAlignment.Top,
        Child = content
    };
}
