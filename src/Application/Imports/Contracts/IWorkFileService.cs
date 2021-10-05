using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Imports.Contracts
{
    public interface IWorkFileService
    {
        Task<bool> Initialize(string path, CancellationToken cancellationToken);
    }
}
