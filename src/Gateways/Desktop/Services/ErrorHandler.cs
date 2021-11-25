using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using EnduranceJudge.Gateways.Desktop.Core.Objects;
using EnduranceJudge.Gateways.Desktop.Views.Error;
using Prism.Events;
using Prism.Services.Dialogs;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IDialogService dialogService;
        public ErrorHandler(IEventAggregator eventAggregator, IDialogService dialogService)
        {
            this.eventAggregator = eventAggregator;
            this.dialogService = dialogService;
        }

        public void Handle(Exception exception)
        {
            exception = GetInnermostException(exception);
            var parameters = new DialogParameters()
                .SetTitle("Error")
                .SetSeverity(MessageSeverity.Error)
                .SetMessage(exception.ToString());
            this.dialogService.ShowDialog(nameof(MessageDialog), parameters, _ => { });
        }

        protected static Exception GetInnermostException(Exception exception)
        {
            if (exception.InnerException != null)
            {
                return GetInnermostException(exception.InnerException);
            }
            return exception;
        }
    }

    public interface IErrorHandler : IService
    {
        void Handle(Exception exception);
    }
}
