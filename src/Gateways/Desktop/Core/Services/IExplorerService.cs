using EnduranceJudge.Core.ConventionalServices;

namespace EnduranceJudge.Gateways.Desktop.Core.Services
{
    public interface IExplorerService : IService
    {
        string SelectDirectory();

        string SelectFile();
    }
}
