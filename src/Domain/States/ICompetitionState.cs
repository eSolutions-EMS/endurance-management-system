using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;

namespace EnduranceJudge.Domain.States
{
    public interface ICompetitionState : IDomainModelState
    {
        CompetitionType Type { get; }
        string Name { get; }
    }
}
