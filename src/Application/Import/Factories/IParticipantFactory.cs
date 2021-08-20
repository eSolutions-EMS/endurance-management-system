using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Aggregates.Common.Athletes;
using EnduranceJudge.Domain.Aggregates.Common.Horses;
using EnduranceJudge.Domain.Aggregates.Import.Participants;

namespace EnduranceJudge.Application.Import.Factories
{
    public interface IParticipantFactory : IService
    {
        Participant Create(Athlete athlete, Horse horse);
    }
}
