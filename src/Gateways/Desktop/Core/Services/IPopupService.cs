using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using EnduranceJudge.Gateways.Desktop.Core.Objects;
using EnduranceJudge.Gateways.Desktop.Views.Dialogs.Confirmation;
using EnduranceJudge.Gateways.Desktop.Views.Dialogs.Message;
using EnduranceJudge.Gateways.Desktop.Views.Dialogs.Startlists;
using Prism.Services.Dialogs;
using System;

namespace EnduranceJudge.Gateways.Desktop.Core.Services;

public class PopupService : IPopupService
{
    private readonly IDialogService dialogService;

    public PopupService(IDialogService dialogService)
    {
        this.dialogService = dialogService;
    }

    public void RenderError(string message)
    {
        var parameters = new DialogParameters()
            .SetSeverity(MessageSeverity.Error)
            .SetMessage(message);
        this.RenderDialog(nameof(MessageDialog), parameters);
    }

    public void RenderValidation(string message)
    {
        var parameters = new DialogParameters()
            .SetSeverity(MessageSeverity.Warning)
            .SetMessage(message);
        this.RenderDialog(nameof(MessageDialog), parameters);
    }

    public void RenderConfirmation(string message, Action action)
    {
        var parameters = new DialogParameters()
            .SetMessage(message);
        Action<IDialogResult> dialogAction = dialog =>
        {
            if (dialog.Result == ButtonResult.Yes)
            {
                action();
            }
        };
        this.RenderDialog(nameof(ConfirmationDialog), parameters, dialogAction);
    }
    public void RenderStartList()
    {
        this.RenderDialog(nameof(StartlistDialog), new DialogParameters());
    }

    public void RenderOk()
    {
        var parameters = new DialogParameters().SetMessage("OK");
        this.RenderDialog(nameof(MessageDialog), parameters);
    }

    private void RenderDialog(string name, IDialogParameters parameters, Action<IDialogResult> action = null)
    {
        action ??= _ => { };
        this.dialogService.ShowDialog(name, parameters, action);
    }
}

public interface IPopupService : ITransientService
{
    void RenderError(string message);
    void RenderValidation(string message);
    void RenderConfirmation(string message, Action action);
    void RenderStartList();
    void RenderOk();
}
