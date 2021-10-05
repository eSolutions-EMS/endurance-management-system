using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Domain.State.Horses;

namespace EnduranceJudge.Application.Events.Commands.Horses
{
    public class RemoveHorse : IdentifiableRequest
    {
        public class RemoveHorseHandler
        {
        }
    }
}
