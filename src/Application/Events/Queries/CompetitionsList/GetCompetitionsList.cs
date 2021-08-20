using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Domain.Aggregates.Event.Competitions;
using MediatR;
using System.Collections.Generic;

namespace EnduranceJudge.Application.Events.Queries.CompetitionsList
{
    public class GetCompetitionsList : IRequest<IEnumerable<ListItemModel>>
    {
        public class GetCompetitionsListHandler : GetAllHandler<GetCompetitionsList, ListItemModel, Competition>
        {
            public GetCompetitionsListHandler(IQueriesBase<Competition> queries) : base(queries)
            {
            }
        }
    }
}
