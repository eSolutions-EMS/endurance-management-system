using System;
using System.Threading.Tasks;

namespace EnduranceJudge.Core.Services
{
    public interface IInitializerInterface
    {
        int RunningOrder { get; }

        void Run(IServiceProvider serviceProvider);
    }
}
