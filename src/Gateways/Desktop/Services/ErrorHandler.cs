using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Gateways.Desktop.Core.Objects;
using Prism.Events;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly IEventAggregator eventAggregator;
        public ErrorHandler(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public void Handle(Exception exception)
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

    public interface IErrorHandler : IService
    {
        void Handle(Exception exception);
    }
}
