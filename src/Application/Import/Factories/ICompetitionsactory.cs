using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Aggregates.Import.Competitions;
using EnduranceJudge.Domain.Aggregates.Import.Participants;
using System.Collections.Generic;

namespace EnduranceJudge.Application.Import.Factories
{
    public interface ICompetitionFactory : IService
    {
        Competition Create(string name, List<Participant> participants);
    }
}
