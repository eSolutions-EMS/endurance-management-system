using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Domain.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Core.Handlers
{
    public class GetOneHandler<TRequest, TResponse, TDomainModel> : Handler<TRequest, TResponse>
        where TRequest : IdentifiableRequest<TResponse>
        where TDomainModel : IDomainModel
    {
        public GetOneHandler()
        {
        }

        public override async Task<TResponse> Handle(TRequest request, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
