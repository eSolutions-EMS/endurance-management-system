using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.States
{
    public interface IPhaseState : IDomainModelState
    {
        int LengthInKm { get; }

        bool IsFinal { get; }
    }
}
