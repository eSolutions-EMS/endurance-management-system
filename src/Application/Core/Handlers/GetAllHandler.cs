using EnduranceJudge.Domain.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Core.Handlers
{
    public class GetAllHandler<TRequest, TResponseModel, TDomainModel> : Handler<TRequest, IEnumerable<TResponseModel>>
        where TRequest : IRequest<IEnumerable<TResponseModel>>
        where TDomainModel : IDomainModel
    {
        public GetAllHandler()
        {
        }

        public override async Task<IEnumerable<TResponseModel>> Handle(TRequest request, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
