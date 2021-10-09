using EnduranceJudge.Domain.State.EnduranceEvents;

namespace EnduranceJudge.Application.Aggregates.Configurations.Contracts
{
    public interface IEnduranceEventQuery
    {
        EnduranceEvent Get();
    }
}
