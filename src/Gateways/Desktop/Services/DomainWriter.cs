using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Domain.Core.Models;
using Prism.Events;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services
{
    public class DomainWriter<T> : DomainReader, IDomainHandler<T>, IServiceExecutor
        where T : IAggregateRoot, new()
    {
        private readonly IPersistence persistence;

        public DomainWriter(IEventAggregator eventAggregator, IPersistence persistence) : base(eventAggregator)
        {
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
                this.Handle(exception);
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
                this.Handle(exception);
                return null;
            }
        }
        public void Execute(Action action)
        {
            try
            {
                action();
                this.persistence.Snapshot();
            }
            catch (Exception exception)
            {
                this.Handle(exception);
            }
        }
    }

    public interface IDomainHandler<T> : IDomainReader
        where T : IAggregateRoot, new()
    {
        public void Write(Action<T> action);
        public IDomainObject Write(Func<T, IDomainObject> action);
    }

    public interface IServiceExecutor : IDomainReader
    {
        public void Execute(Action action);
    }
}
