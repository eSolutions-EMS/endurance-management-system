using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EnduranceJudge.Application.Core.Handlers
{
    public abstract class Handler<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        public abstract Task DoHandle(TRequest request, CancellationToken token);

        public async Task<Unit> Handle(TRequest request, CancellationToken token)
        {
            await this.DoHandle(request, token);
            return new Unit();
        }
    }

    public abstract class Handler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken token);
    }
}
