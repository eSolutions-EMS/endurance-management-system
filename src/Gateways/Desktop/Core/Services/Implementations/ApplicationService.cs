using EnduranceJudge.Application.Core.Exceptions;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Gateways.Desktop.Core.Events;
using MediatR;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Desktop.Core.Services.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly IMediator mediator;
        private readonly IEventAggregator eventAggregator;

        public ApplicationService(IMediator mediator, IEventAggregator eventAggregator)
        {
            this.mediator = mediator;
            this.eventAggregator = eventAggregator;
        }

        public async Task<T> Execute<T>(IRequest<T> request)
        {
            try
            {
                return await this.mediator.Send(request);
            }
            catch (DomainException exception)
            {
                this.Publish<ValidationErrorEvent>(exception.Message);
                throw;
            }
            catch (AppException exception)
            {
                this.Publish<ValidationErrorEvent>(exception.Message);
                throw;
            }
            catch (Exception exception)
            {
                this.Publish<ValidationErrorEvent>(exception.InnerException?.Message ?? exception.Message);
                throw;
            }
        }

        public void Publish<T>(string message)
            where T : PubSubEvent<string>, new()
        {
            this.eventAggregator
                .GetEvent<T>()
                .Publish(message);
        }
    }
}
