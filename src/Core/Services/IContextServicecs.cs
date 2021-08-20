using EnduranceJudge.Core.ConventionalServices;
using System.Threading.Tasks;

namespace EnduranceJudge.Core.Services
{
    public interface IContextService : ISingletonService
    {
        Task Load();
    }
}
