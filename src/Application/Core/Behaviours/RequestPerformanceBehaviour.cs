using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EnduranceJudge.Application.Core.Behaviours
{
    public class RequestPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            return await next();
        }
    }
}
