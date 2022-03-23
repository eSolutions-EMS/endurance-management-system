using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Core.ConventionalServices;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services;

public class Executor : IExecutor
{
    private readonly IErrorHandler errorHandler;
    private readonly IPersistence persistence;

    public Executor(IErrorHandler errorHandler, IPersistence persistence)
    {
        this.errorHandler = errorHandler;
        this.persistence = persistence;
    }

    public bool Execute(Action action)
    {
        try
        {
            action();
            this.persistence.Snapshot();
            return true;
        }
        catch (Exception exception)
        {
            this.errorHandler.Handle(exception);
            return false;
        }
    }

    public TResult Execute<TResult>(Func<TResult> action)
    {
        try
        {
            var result = action();
            this.persistence.Snapshot();
            return result;
        }
        catch (Exception exception)
        {
            this.errorHandler.Handle(exception);
            return default;
        }
    }
}

public interface IExecutor : IService
{
    public bool Execute(Action action);
    TResult Execute<TResult>(Func<TResult> action);
}
