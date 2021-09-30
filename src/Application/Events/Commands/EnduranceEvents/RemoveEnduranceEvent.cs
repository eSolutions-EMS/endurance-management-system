using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Domain.Aggregates.Event.EnduranceEvents;

namespace EnduranceJudge.Application.Events.Commands.EnduranceEvents
{
    public class RemoveEnduranceEvent : IdentifiableRequest
    {
        public class RemoveEnduranceEventHandler : RemoveOneHandler<RemoveEnduranceEvent, EnduranceEvent>
        {
            public RemoveEnduranceEventHandler(ICommands<EnduranceEvent> commands) : base(commands)
            {
            }
        }
    }
}
