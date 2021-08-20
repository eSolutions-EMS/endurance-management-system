using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Import.Contracts
{
    public interface IWorkFileService
    {
        Task<bool> Initialize(string path, CancellationToken cancellationToken);
    }
}
