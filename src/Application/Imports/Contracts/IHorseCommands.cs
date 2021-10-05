using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Domain.State.Horses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Imports.Contracts
{
    public interface IHorseCommands : ICommands<Horse>
    {
        Task Create(IEnumerable<Horse> domainModels, CancellationToken token);
    }
}
