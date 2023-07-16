using EMS.Judge.Application.Services;
using Core.ConventionalServices;
using System;
using System.Threading.Tasks;

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

    public async Task<bool> Execute(Func<T, Task> action, bool persist)
    {
        var innerAction = async () => await action(this.service);
        return await this.executor.Execute(innerAction, persist);
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

    public async Task<bool> Execute(Func<Task> action, bool persist)
    {
        try
        {
            await action();
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
    Task<bool> Execute(Func<T, Task> action, bool persist);
    TResult Execute<TResult>(Func<T, TResult> action, bool persist);
}
public interface IExecutor : ITransientService
{
    public bool Execute(Action action, bool persist);
    Task<bool> Execute(Func<Task> action, bool persist);
    TResult Execute<TResult>(Func<TResult> action, bool persist);
}
