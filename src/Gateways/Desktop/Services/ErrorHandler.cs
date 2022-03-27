using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Application.Core.Exceptions;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System;
using static EnduranceJudge.Localization.Strings;

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
                var logFile = this.persistence.LogError(exception.ToString());
                var message = string.Format(UNEXPECTED_ERROR_MESSAGE, logFile);
                this.popupService.RenderError(message);
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

public interface IErrorHandler : ITransientService
{
    void Handle(Exception exception);
}
