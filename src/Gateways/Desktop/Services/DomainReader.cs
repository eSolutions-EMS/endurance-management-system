using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Gateways.Desktop.Core.Objects;
using Prism.Events;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services
{
    public class DomainReader : IDomainReader
    {
        private readonly IEventAggregator eventAggregator;

        public DomainReader(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public void Read(Action action)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                this.Handle(exception);
            }
        }

        protected void Handle(Exception exception)
        {
            exception = GetInnermostException(exception);
            this.eventAggregator
                .GetEvent<ErrorEvent>()
                .Publish(exception.ToString());
        }

        protected static Exception GetInnermostException(Exception exception)
        {
            if (exception.InnerException != null)
            {
                return GetInnermostException(exception.InnerException);
            }
            return exception;
        }
    }

    public interface IDomainReader : IService
    {
        public void Read(Action action);
    }
}
