using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly IPopupService popupService;

        public ErrorHandler(IPopupService popupService)
        {
            this.popupService = popupService;
            this.popupService = popupService;
        }

        public void Handle(Exception exception)
        {
            exception = GetInnermostException(exception);
            if (exception is DomainException)
            {
                this.popupService.RenderValidation(exception.Message);
            }
            else
            {
                this.popupService.RenderError(exception.ToString());
            }
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
