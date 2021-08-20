using EnduranceJudge.Application.Core.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EnduranceJudge.Application.Core
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
            => services
                .AddMediatR(ApplicationConstants.Assemblies)
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
    }
}
