using EnduranceJudge.Core.ConventionalServices;

namespace EnduranceJudge.Gateways.Desktop.Services
{
    public class ApplicationContext : IApplicationContext
    {
        public bool IsInitialized { get; private set; }

        public void Initialize()
        {
            this.IsInitialized = true;
        }
    }

    public interface IApplicationContext : ISingletonService
    {
        bool IsInitialized { get; }
        void Initialize();
    }
}
