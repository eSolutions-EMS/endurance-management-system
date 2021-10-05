using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Imports.Contracts
{
    public interface IStorageInitializer
    {
        Task<bool> Initialize(string path, CancellationToken token);
    }
}
