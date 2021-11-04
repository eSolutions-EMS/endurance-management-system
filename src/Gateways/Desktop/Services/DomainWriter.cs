using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Domain.Core.Models;
using Prism.Events;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services
{
    public class DomainWriter<T> : DomainReader, IDomainHandler<T>
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
    }

    public interface IDomainHandler<T> : IDomainReader
        where T : IAggregateRoot, new()
    {
        public void Write(Action<T> action);
    }
}
