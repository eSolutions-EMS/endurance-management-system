using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Aggregates.Event.Competitions;

namespace EnduranceJudge.Application.Events.Factories
{
    public interface ICompetitionFactory : IService
    {
        Competition Create(CompetitionDependantModel data);
    }
}
