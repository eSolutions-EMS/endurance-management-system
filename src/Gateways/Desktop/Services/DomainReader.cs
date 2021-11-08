using EnduranceJudge.Core.ConventionalServices;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services
{
    public class DomainReader : IDomainReader
    {
        private readonly IErrorHandler errorHandler;

        public DomainReader(IErrorHandler errorHandler)
        {
            this.errorHandler = errorHandler;
        }

        public void Read(Action action)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                this.errorHandler.Handle(exception);
            }
        }
    }

    public interface IDomainReader : IService
    {
        public void Read(Action action);
    }
}
