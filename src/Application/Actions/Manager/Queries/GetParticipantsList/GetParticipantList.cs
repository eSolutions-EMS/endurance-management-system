using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Domain.Aggregates.Manager.Participants;
using MediatR;
using System.Collections.Generic;

namespace EnduranceJudge.Application.Actions.Manager.Queries.GetParticipantsList
{
    public class GetParticipantList : IRequest<IEnumerable<ListItemModel>>
    {
        public class GetParticipantListHandler : GetAllHandler<GetParticipantList, ListItemModel, Participant>
        {
            public GetParticipantListHandler(IQueries<Participant> queries) : base(queries)
            {
            }
        }
    }
}
