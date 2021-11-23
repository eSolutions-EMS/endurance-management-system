using EnduranceJudge.Core.ConventionalServices;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services
{
    public class BasicExecutor : IBasicExecutor
    {
        private readonly IErrorHandler errorHandler;

        public BasicExecutor(IErrorHandler errorHandler)
        {
            this.errorHandler = errorHandler;
        }

        public bool Execute(Action action)
        {
            try
            {
                action();
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
