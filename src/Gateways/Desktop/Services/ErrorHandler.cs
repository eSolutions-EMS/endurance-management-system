using EnduranceJudge.Application.Services;
using EnduranceJudge.Application.Core.Exceptions;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Core.Events;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System;
using static EnduranceJudge.Gateways.Desktop.DesktopConstants;

namespace EnduranceJudge.Gateways.Desktop.Services;

public class ErrorHandler : IErrorHandler
{
    private readonly IPopupService popupService;
    private readonly IPersistence persistence;

    public ErrorHandler(IPopupService popupService, IPersistence persistence)
    {
        this.popupService = popupService;
        this.persistence = persistence;
        this.popupService = popupService;
        CoreEvents.ErrorEvent += (_, exception) => this.Handle(exception);
    }

    public void Handle(Exception exception)
    {
        exception = GetInnermostException(exception);
        if (exception is DomainExceptionBase or DomainExceptionBase or AppException)
        {
            this.popupService.RenderValidation(exception.Message);
        }
        else
        {
            # if DEBUG
                this.popupService.RenderError(exception.ToString());
            # else
                var logFile = this.persistence.LogError(exception.Message, exception.StackTrace);
                var message = string.Format(UNEXPECTED_ERROR_MESSAGE, logFile);
                this.popupService.RenderError(message);
                this.popupService.RenderError(exception.ToString());
            # endif
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

public interface IErrorHandler : ISingletonService
{
    void Handle(Exception exception);
}
