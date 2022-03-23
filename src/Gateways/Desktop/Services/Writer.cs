using EnduranceJudge.Core.ConventionalServices;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services;

public class Executor<T> : IExecutor<T>
    where T : IService
{
    private readonly T service;
    private readonly IBasicExecutor basicExecutor;

    public Executor(T service, IBasicExecutor basicExecutor)
    {
        this.service = service;
        this.basicExecutor = basicExecutor;
    }

    public bool Execute(Action<T> action)
    {
        var innerAction = () => action(this.service);
        return this.basicExecutor.Execute(innerAction);
    }

    public TResult Execute<TResult>(Func<T, TResult> action)
    {
        var innerAction = () => action(this.service);
        return this.basicExecutor.Execute(innerAction);
    }
}

public interface IExecutor<out T>
    where T : IService
{
    bool Execute(Action<T> action);
    TResult Execute<TResult>(Func<T, TResult> action);
}
