using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Domain.Core.Models;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services
{
    public class DomainExecutor<T> : DomainReader, IDomainExecutor<T>
        where T : IAggregateRoot, new()
    {
        private readonly IErrorHandler errorHandler;
        private readonly IPersistence persistence;

        public DomainExecutor(IErrorHandler errorHandler, IPersistence persistence) : base(errorHandler)
        {
            this.errorHandler = errorHandler;
            this.persistence = persistence;
        }

        public void Write(Action<T> action)
        {
            try
            {
                var manager = new T();
                action(manager);
                this.persistence.Snapshot();
            }
            catch (Exception exception)
            {
                this.errorHandler.Handle(exception);
            }
        }

        public IDomainObject Write(Func<T, IDomainObject> action)
        {
            try
            {
                var manager = new T();
                var result = action(manager);
                this.persistence.Snapshot();
                return result;
            }
            catch (Exception exception)
            {
                this.errorHandler.Handle(exception);
                return null;
            }
        }
    }

    public interface IDomainExecutor<T> : IDomainReader
        where T : IAggregateRoot, new()
    {
        public void Write(Action<T> action);
        public IDomainObject Write(Func<T, IDomainObject> action);
    }
}
