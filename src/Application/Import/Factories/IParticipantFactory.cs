using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Domain.Aggregates.Import.Participants;

namespace EnduranceJudge.Application.Import.Factories
{
    public interface IParticipantFactory : IService
    {
        Participant Create(Athlete athlete, Horse horse);
    }
}
