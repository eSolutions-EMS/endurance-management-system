using EMS.Judge.Common.Objects;
using EMS.Judge.Common.ViewModels;
using EMS.Judge.Common.Extensions;
using static Core.Localization.Strings;
using Prism.Services.Dialogs;

namespace EMS.Judge.Views.Dialogs.Message;

public class MessageDialogModel : DialogBase
{
    private string message;
    private MessageSeverity severity;

    public override void OnDialogOpened(IDialogParameters parameters)
    {
        this.Severity = parameters.GetSeverity();
        this.Message = parameters.GetMessage();
        this.Title = this.Severity == MessageSeverity.Error
            ? APPLICATION_ERROR
            : VALIDATION_MESSAGE;
    }

    public string Message
    {
        get => this.message;
        set => this.SetProperty(ref this.message, value);
    }
    public MessageSeverity Severity
    {
        get => this.severity;
        set => this.SetProperty(ref this.severity, value);
    }
}
