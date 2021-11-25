using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using EnduranceJudge.Gateways.Desktop.Core.Objects;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;

namespace EnduranceJudge.Gateways.Desktop.Views.Error
{
    public class MessageDialogModel : ViewModelBase, IDialogAware
    {
        public override void OnNavigatedTo(NavigationContext context)
        {
            base.OnNavigatedTo(context);
            this.Message = context.GetMessage();
        }

        private string message;
        private MessageSeverity severity;
        public string Title { get; private set; }

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

        public bool CanCloseDialog() => true;
        public void OnDialogClosed()
        {
        }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            this.Title = parameters.GetTitle();
            this.Severity = parameters.GetSeverity();
            this.Message = parameters.GetMessage();
        }

        public event Action<IDialogResult> RequestClose;
    }
}
