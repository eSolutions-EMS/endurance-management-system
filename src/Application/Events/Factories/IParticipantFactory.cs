using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Aggregates.State.Participants;

namespace EnduranceJudge.Application.Events.Factories
{
    public interface IParticipantFactory : IService
    {
        Participant Create(ParticipantDependantModel data);
    }
}
