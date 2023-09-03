using EMS.Judge.Views.Dialogs.Confirmation;
using EMS.Judge.Views.Dialogs.Message;
using EMS.Judge.Views.Dialogs.Startlists;
using Core.ConventionalServices;
using EMS.Judge.Common.Objects;
using EMS.Judge.Common.Extensions;
using Prism.Services.Dialogs;
using System;
using Core.Application.Services;

namespace EMS.Judge.Common.Services;

public class PopupService : IPopupService, INotificationService
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

    public void Error(string message)
    {
        this.RenderError(message);
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
