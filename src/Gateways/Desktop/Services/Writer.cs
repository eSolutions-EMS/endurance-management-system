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

public interface IExecutor<out T>
    where T : IService
{
    bool Execute(Action<T> action);
    TResult Execute<TResult>(Func<T, TResult> action);
}
