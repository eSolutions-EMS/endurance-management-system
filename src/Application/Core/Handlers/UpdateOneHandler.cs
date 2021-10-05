using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Core.Handlers
{
    public class UpdateOneHandler<TRequest, TDomainModel> : Handler<TRequest>
        where TRequest : IdentifiableRequest, IMapTo<TDomainModel>
        where TDomainModel : IAggregateRoot
    {
        protected UpdateOneHandler()
        {
        }

        public override async Task DoHandle(TRequest request, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
