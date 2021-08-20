using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Domain.Aggregates.Common.Athletes;

namespace EnduranceJudge.Application.Contracts.Athletes
{
    public interface IAthleteCommands : ICommandsBase<Athlete>
    {
    }
}
