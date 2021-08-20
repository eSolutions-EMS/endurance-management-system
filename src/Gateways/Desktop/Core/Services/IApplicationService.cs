using EnduranceJudge.Core.ConventionalServices;
using MediatR;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Desktop.Core.Services
{
    public interface IApplicationService : IService
    {
        Task<T> Execute<T>(IRequest<T> request);
    }
}
