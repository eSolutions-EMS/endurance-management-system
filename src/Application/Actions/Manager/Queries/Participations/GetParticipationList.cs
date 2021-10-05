using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Domain.State.Participations;
using MediatR;
using System.Collections.Generic;

namespace EnduranceJudge.Application.Actions.Manager.Queries.Participations
{
    public class GetParticipationList : IRequest<IEnumerable<ListItemModel>>
    {
        public class GetParticipationListHandler : GetAllHandler<GetParticipationList, ListItemModel, Participation>
        {
            public GetParticipationListHandler() : base()
            {
            }
        }
    }
}
