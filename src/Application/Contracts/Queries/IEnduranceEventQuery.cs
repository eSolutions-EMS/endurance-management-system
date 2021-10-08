using EnduranceJudge.Domain.State.EnduranceEvents;

namespace EnduranceJudge.Application.Contracts.Queries
{
    public interface IEnduranceEventQuery
    {
        EnduranceEvent Get();
    }
}
