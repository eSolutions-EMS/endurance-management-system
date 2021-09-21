using EnduranceJudge.Domain.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Core.Contracts
{
    public interface ICommands<TDomainModel> : IQueries<TDomainModel>
        where TDomainModel : IDomainModel
    {
        Task Save(TDomainModel domainModel, CancellationToken cancellationToken);

        Task<T> Save<T>(TDomainModel domainModel, CancellationToken cancellationToken);
    }
}
