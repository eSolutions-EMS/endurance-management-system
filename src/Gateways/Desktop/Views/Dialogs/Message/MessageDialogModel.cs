using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using EnduranceJudge.Gateways.Desktop.Core.Objects;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using Prism.Services.Dialogs;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Gateways.Desktop.Views.Dialogs.Message
{
    public class MessageDialogModel : DialogBase
    {
        private string message;
        private MessageSeverity severity;

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            this.Severity = parameters.GetSeverity();
            this.Message = parameters.GetMessage();
            this.Title = this.Severity == MessageSeverity.Error
                ? DesktopStrings.APPLICATION_ERROR
                : DesktopStrings.VALIDATION_TITLE;
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
}
