using EnduranceJudge.Core.ConventionalServices;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Persistence.Services
{
    public interface ISeederService : IService
    {
        Task Seed();
    }
}
