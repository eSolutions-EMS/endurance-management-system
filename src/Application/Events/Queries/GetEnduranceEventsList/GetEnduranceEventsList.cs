using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Domain.State;
using MediatR;
using System.Collections.Generic;

namespace EnduranceJudge.Application.Events.Queries.GetEnduranceEventsList
{
    public class GetEnduranceEventsList : IRequest<IEnumerable<ListItemModel>>
    {
        public class GetEnduranceEventsListHandler
            : GetAllHandler<GetEnduranceEventsList, ListItemModel, EventState>
        {
            public GetEnduranceEventsListHandler(IQueries<EventState> queries) : base(queries)
            {
            }
        }
    }
}
