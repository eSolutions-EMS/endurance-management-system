using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using static EnduranceJudge.Localization.Strings;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;

namespace EnduranceJudge.Gateways.Desktop.Views.Dialogs.Confirmation
{
    public class ConfirmationDialogModel : DialogBase
    {
        private Action action;

        public ConfirmationDialogModel()
        {
            this.Confirm = new DelegateCommand(this.ConfirmAction);
            this.Reject = new DelegateCommand(this.RejectAction);
        }

        public DelegateCommand Confirm { get; }
        public DelegateCommand Reject { get; }
        private string message;

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            this.Title = CONFIRMATION;
            this.Message = parameters.GetMessage();
        }

        private void ConfirmAction()
        {
            var result = new DialogResult(ButtonResult.Yes);
            this.Close(result);
        }

        private void RejectAction()
        {
            var result = new DialogResult(ButtonResult.No);
            this.Close(result);
        }

        public string Message
        {
            get => this.message;
            private set => this.SetProperty(ref this.message, value);
        }
    }
}
