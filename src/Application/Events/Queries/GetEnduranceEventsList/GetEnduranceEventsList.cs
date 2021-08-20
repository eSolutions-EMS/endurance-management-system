using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.Models;
using EnduranceJudge.Domain.Aggregates.Event.EnduranceEvents;
using MediatR;
using System.Collections.Generic;

namespace EnduranceJudge.Application.Events.Queries.GetEnduranceEventsList
{
    public class GetEnduranceEventsList : IRequest<IEnumerable<ListItemModel>>
    {
        public class GetEnduranceEventsListHandler
            : GetAllHandler<GetEnduranceEventsList, ListItemModel, EnduranceEvent>
        {
            public GetEnduranceEventsListHandler(IQueriesBase<EnduranceEvent> queries) : base(queries)
            {
            }
        }
    }
}
