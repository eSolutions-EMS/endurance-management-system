using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Domain.Aggregates.Common.Athletes;

namespace EnduranceJudge.Application.Events.Commands.Athletes
{
    public class RemoveAthlete : IdentifiableRequest
    {
        public class RemoveAthleteHandler : RemoveOneHandler<RemoveAthlete, Athlete>
        {
            public RemoveAthleteHandler(ICommands<Athlete> commands) : base(commands)
            {
            }
        }
    }
}
