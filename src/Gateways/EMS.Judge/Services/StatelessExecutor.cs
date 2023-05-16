using EnduranceJudge.Core.ConventionalServices;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services;

public class StatelessExecutor : IStatelessExecutor
{
    private readonly IErrorHandler errorHandler;
    public StatelessExecutor(IErrorHandler errorHandler)
    {
        this.errorHandler = errorHandler;
    }

    public bool Execute(Action action)
    {
        try
        {
            action();
            return true;
        }
        catch (Exception exception)
        {
            this.errorHandler.Handle(exception);
            return false;
        }
    }
}

public interface IStatelessExecutor : ITransientService
{
    public bool Execute(Action action);
}
