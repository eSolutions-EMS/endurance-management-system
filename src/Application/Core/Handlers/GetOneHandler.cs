using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Domain.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Core.Handlers
{
    public class GetOneHandler<TRequest, TResponse, TDomainModel> : Handler<TRequest, TResponse>
        where TRequest : IdentifiableRequest<TResponse>
        where TDomainModel : IDomainModel
    {
        private readonly IQueriesBase<TDomainModel> query;

        public GetOneHandler(IQueriesBase<TDomainModel> query)
        {
            this.query = query;
        }

        public override async Task<TResponse> Handle(TRequest request, CancellationToken token)
        {
            var entity = await this.query.Find<TResponse>(request.Id);
            return entity;
        }
    }
}
