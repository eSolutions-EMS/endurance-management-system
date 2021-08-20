using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Domain.Aggregates.Common.Athletes;
using MediatR;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EnduranceJudge.Application.Events.Queries.GetAthletesList
{
    public class GetAthletesList : IRequest<IEnumerable<ListItemModel>>
    {
        public class GetAthletesHandler : GetAllHandler<GetAthletesList, ListItemModel, Athlete>
        {
            public GetAthletesHandler(IQueriesBase<Athlete> queries) : base(queries)
            {
            }
        }
    }
}
