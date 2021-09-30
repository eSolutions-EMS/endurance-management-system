using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Domain.Aggregates.Common.Horses;

namespace EnduranceJudge.Application.Events.Commands.Horses
{
    public class RemoveHorse : IdentifiableRequest
    {
        public class RemoveHorseHandler : RemoveOneHandler<RemoveHorse, Horse>
        {
            public RemoveHorseHandler(ICommands<Horse> commands) : base(commands)
            {
            }
        }
    }
}
