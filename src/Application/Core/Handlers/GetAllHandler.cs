using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Domain.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Core.Handlers
{
    public class GetAllHandler<TRequest, TResponseModel, TDomainModel> : Handler<TRequest, IEnumerable<TResponseModel>>
        where TRequest : IRequest<IEnumerable<TResponseModel>>
        where TDomainModel : IDomainModel
    {
        private readonly IQueries<TDomainModel> queries;

        public GetAllHandler(IQueries<TDomainModel> queries)
        {
            this.queries = queries;
        }

        public override async Task<IEnumerable<TResponseModel>> Handle(TRequest request, CancellationToken token)
        {
            var result = await this.queries.All<TResponseModel>();
            return result;
        }
    }
}
