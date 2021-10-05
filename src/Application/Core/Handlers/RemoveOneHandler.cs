using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Domain.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Core.Handlers
{
    public class RemoveOneHandler<TRequest, TDomainModel> : Handler<TRequest>
        where TRequest : IdentifiableRequest
        where TDomainModel : IAggregateRoot
    {
        public RemoveOneHandler()
        {
        }

        public override async Task DoHandle(TRequest request, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
