using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Core.ConventionalServices;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services
{
    public class ServiceExecutor : IServiceExecutor
    {
        private readonly IErrorHandler errorHandler;
        private readonly IPersistence persistence;

        public ServiceExecutor(IErrorHandler errorHandler, IPersistence persistence)
        {
            this.errorHandler = errorHandler;
            this.persistence = persistence;
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
                this.errorHandler.Handle(exception);
            }
        }
    }

    public interface IServiceExecutor : IService
    {
        public void Execute(Action action);
    }
}
