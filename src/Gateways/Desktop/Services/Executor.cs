using EnduranceJudge.Application.Services;
using EnduranceJudge.Core.ConventionalServices;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services;

public class Executor<T> : IExecutor<T>
    where T : IService
{
    private readonly T service;
    private readonly IExecutor executor;

    public Executor(T service, IExecutor executor)
    {
        this.service = service;
        this.executor = executor;
    }

    public bool Execute(Action<T> action)
    {
        var innerAction = () => action(this.service);
        return this.executor.Execute(innerAction);
    }

    public TResult Execute<TResult>(Func<T, TResult> action)
    {
        var innerAction = () => action(this.service);
        return this.executor.Execute(innerAction);
    }
}

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

public interface IExecutor<out T> : ITransientService
    where T : IService
{
    bool Execute(Action<T> action);
    TResult Execute<TResult>(Func<T, TResult> action);
}
public interface IExecutor : ITransientService
{
    public bool Execute(Action action);
    TResult Execute<TResult>(Func<TResult> action);
}
