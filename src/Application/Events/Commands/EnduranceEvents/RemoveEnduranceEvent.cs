using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Domain.State;

namespace EnduranceJudge.Application.Events.Commands.EnduranceEvents
{
    public class RemoveEnduranceEvent : IdentifiableRequest
    {
        public class RemoveEnduranceEventHandler : RemoveOneHandler<RemoveEnduranceEvent, EventState>
        {
            public RemoveEnduranceEventHandler(ICommands<EventState> commands) : base(commands)
            {
            }
        }
    }
}
