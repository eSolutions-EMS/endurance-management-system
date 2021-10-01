using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Domain.Aggregates.Manager.Participations;
using MediatR;
using System.Collections.Generic;

namespace EnduranceJudge.Application.Actions.Manager.Queries.Participations
{
    public class GetParticipationList : IRequest<IEnumerable<ListItemModel>>
    {
        public class GetParticipationListHandler : GetAllHandler<GetParticipationList, ListItemModel, Participation>
        {
            public GetParticipationListHandler(IQueries<Participation> queries) : base(queries)
            {
            }
        }
    }
}
