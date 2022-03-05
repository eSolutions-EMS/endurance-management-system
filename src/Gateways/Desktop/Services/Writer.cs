using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Domain.Core.Models;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services
{
    public class Executor<T> : IExecutor<T>
    {
        private readonly T service;
        private readonly IErrorHandler errorHandler;
        private readonly IPersistence persistence;

        public Executor(T service, IErrorHandler errorHandler, IPersistence persistence)
        {
            this.service = service;
            this.errorHandler = errorHandler;
            this.persistence = persistence;
        }

        public bool Execute(Action<T> action)
        {
            try
            {
                action(this.service);
                this.persistence.Snapshot();
                return true;
            }
            catch (Exception exception)
            {
                this.errorHandler.Handle(exception);
                return false;
            }
        }

        public TResult Execute<TResult>(Func<T, TResult> action)
        {
            try
            {
                var result = action(this.service);
                this.persistence.Snapshot();
                return result;
            }
            catch (Exception exception)
            {
                this.errorHandler.Handle(exception);
                return default;
            }
        }

        public (bool, IDomain) Execute(Func<T, IDomain> action)
        {
            try
            {
                var result = action(this.service);
                this.persistence.Snapshot();
                return (true, result);
            }
            catch (Exception exception)
            {
                this.errorHandler.Handle(exception);
                return (false, null);
            }
        }
    }

    public interface IExecutor<T>
    {
        bool Execute(Action<T> action);
        TResult Execute<TResult>(Func<T, TResult> action);
        (bool, IDomain) Execute(Func<T, IDomain> action);
    }
}
