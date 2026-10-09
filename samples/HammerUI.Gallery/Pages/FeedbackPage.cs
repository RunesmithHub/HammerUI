using Avalonia.Controls;
using HammerUI.Controls;
using HammerUI.Services;
using static HammerUI.Gallery.Pages.Layout;

namespace HammerUI.Gallery.Pages;

/// <summary>Toasts of every kind, message and confirmation dialogs, and an empty state.</summary>
internal static class FeedbackPage
{
    public static Control Create(IToastService toasts, IDialogService dialogs) => Page(
        Card("Toasts", Row(
                Action("Information", () => toasts.Show("Indexing finished", "1,284 files in 0.8 s")),
                Action("Success", () => toasts.Show("Saved", "settings.json", ToastKind.Success)),
                Action("Warning", () => toasts.Show("2 files are missing", "The layout refers to panels that no longer exist.", ToastKind.Warning)),
                Action("Error", () => toasts.Show("Could not save", "The folder is read-only.", ToastKind.Error)),
                Action("With a button", () => toasts.Show("Export finished", "3 files", ToastKind.Success, TimeSpan.FromSeconds(8),
                    new ToastAction("Show them", () => toasts.Show("Here they are", "export/level-01.json, export/level-02.json, export/level-03.json"))))),
            "Toasts stack in the corner of the window and go away on their own. A toast can carry a button for what to do next."),
        Card("Dialogs", Row(
            Action("Message", () => _ = dialogs.ShowMessageAsync("Welcome", "Dialogs dim the window and keep the keyboard inside them.",
                [new MessageDialogButton("Got it", true, IsDefault: true)])),
            Action("Confirm", async () =>
            {
                var delete = await dialogs.ConfirmAsync("Delete 3 files?", "They move to the trash.", "Delete", isDestructive: true);
                toasts.Show(delete ? "Deleted" : "Kept the files", kind: delete ? ToastKind.Success : ToastKind.Info);
            }),
            Action("Unsaved changes", async () =>
            {
                var choice = await dialogs.AskToSaveChangesAsync("notes.md");
                toasts.Show($"You chose {choice}");
            }))),
        Card("Empty state", new EmptyState
        {
            Icon = Icons.Search,
            Title = "No results",
            Hint = "Try a shorter search or another folder.",
            ActionText = "Clear search",
            Height = 220
        }));

    private static Button Action(string text, Action action)
    {
        var button = new Button { Content = text };
        button.Click += (_, _) => action();
        return button;
    }

    private static Button Action(string text, Func<Task> action)
    {
        var button = new Button { Content = text };
        button.Click += async (_, _) => await action();
        return button;
    }
}
