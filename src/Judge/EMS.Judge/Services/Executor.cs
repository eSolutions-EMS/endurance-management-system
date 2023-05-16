using EMS.Judge.Application.Services;
using EMS.Core.ConventionalServices;
using System;

namespace EMS.Judge.Services;

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

    public bool Execute(Action<T> action, bool persist)
    {
        var innerAction = () => action(this.service);
        return this.executor.Execute(innerAction, persist);
    }

    public TResult Execute<TResult>(Func<T, TResult> action, bool persist)
    {
        var innerAction = () => action(this.service);
        return this.executor.Execute(innerAction, persist);
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

    public bool Execute(Action action, bool persist)
    {
        try
        {
            action();
            if (persist)
            {
                this.persistence.SaveState();
            }
            return true;
        }
        catch (Exception exception)
        {
            this.errorHandler.Handle(exception);
            return false;
        }
    }

    public TResult Execute<TResult>(Func<TResult> action, bool persist)
    {
        try
        {
            var result = action();
            if (persist)
            {
                this.persistence.SaveState();
            }
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
    bool Execute(Action<T> action, bool persist);
    TResult Execute<TResult>(Func<T, TResult> action, bool persist);
}
public interface IExecutor : ITransientService
{
    public bool Execute(Action action, bool persist);
    TResult Execute<TResult>(Func<TResult> action, bool persist);
}
