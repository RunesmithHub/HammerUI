namespace HammerUI.Controls;

/// <summary>A button on a toast, such as "Show details", and what it does; clicking it also dismisses the toast.</summary>
public sealed record ToastAction(string Text, Action Invoke);

/// <summary>Converters the toast's template uses for its action.</summary>
public static class ToastActionConverters
{
    /// <summary>Gets the text of a <see cref="ToastAction"/>.</summary>
    public static Avalonia.Data.Converters.FuncValueConverter<ToastAction?, string?> Text { get; } = new(action => action?.Text);
}
