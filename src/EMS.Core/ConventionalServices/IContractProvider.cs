using Microsoft.Extensions.DependencyInjection;

namespace EMS.Core.ConventionalServices;

public interface IContractProvider
{
    IServiceCollection ProvideImplementations(IServiceCollection services);
}
