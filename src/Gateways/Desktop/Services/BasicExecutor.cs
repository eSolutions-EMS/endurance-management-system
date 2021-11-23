using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Core.ConventionalServices;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services
{
    public class BasicExecutor : IBasicExecutor
    {
        private readonly IErrorHandler errorHandler;
        private readonly IPersistence persistence;

        public BasicExecutor(IErrorHandler errorHandler, IPersistence persistence)
        {
            this.errorHandler = errorHandler;
            this.persistence = persistence;
        }

        public bool Execute(Action action)
        {
            try
            {
                action();
                this.persistence.Snapshot();
                return true;
            }
            catch (Exception exception)
            {
                this.errorHandler.Handle(exception);
                return false;
            }
        }
    }

    public interface IBasicExecutor : IService
    {
        public bool Execute(Action action);
    }
}
