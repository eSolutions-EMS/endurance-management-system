using EnduranceJudge.Domain.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Core.Contracts
{
    public interface ICommands<TDomainModel> : IQueries<TDomainModel>
        where TDomainModel : IDomainModel
    {
        Task Save(TDomainModel domain, CancellationToken token);

        Task<T> Save<T>(TDomainModel domain, CancellationToken token);

        Task Remove(int id, CancellationToken token);

        Task Update(IEnumerable<TDomainModel> models, CancellationToken token);
    }
}
