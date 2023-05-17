using Microsoft.Extensions.DependencyInjection;

namespace Core.ConventionalServices;

public interface IContractProvider
{
    IServiceCollection ProvideImplementations(IServiceCollection services);
}
