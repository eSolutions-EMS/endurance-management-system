using EnduranceJudge.Application.Events.Common;
using MediatR;
using System.Collections.Generic;

namespace EnduranceJudge.Application.Events.Queries.GetEnduranceEventsList
{
    public class GetEnduranceEventsList : IRequest<IEnumerable<ListItemModel>>
    {
        public class GetEnduranceEventsListHandler
        {
        }
    }
}
