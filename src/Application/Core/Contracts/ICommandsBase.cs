using EnduranceJudge.Domain.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Core.Contracts
{
    public interface ICommandsBase<TDomainModel> : IQueriesBase<TDomainModel>
        where TDomainModel : IDomainModel
    {
        Task Save(TDomainModel domainModel, CancellationToken cancellationToken);

        Task<T> Save<T>(TDomainModel domainModel, CancellationToken cancellationToken);
    }
}
