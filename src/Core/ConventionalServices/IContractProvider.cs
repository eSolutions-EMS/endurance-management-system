using Microsoft.Extensions.DependencyInjection;

namespace EnduranceJudge.Core.ConventionalServices
{
    public interface IContractProvider
    {
        IServiceCollection ProvideImplementations(IServiceCollection services);
    }
}
