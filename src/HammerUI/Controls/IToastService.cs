namespace HammerUI.Controls;

/// <summary>Shows transient, non-blocking notifications.</summary>
public interface IToastService
{
    /// <summary>Shows a notification that dismisses itself after <paramref name="duration"/>; safe to call from any thread.</summary>
    /// <param name="action">A button the toast shows under its message, or null for none.</param>
    void Show(string title, string? message = null, ToastKind kind = ToastKind.Info, TimeSpan? duration = null, ToastAction? action = null);
}
