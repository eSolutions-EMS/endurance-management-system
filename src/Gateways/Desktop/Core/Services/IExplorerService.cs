using EnduranceJudge.Core.ConventionalServices;

namespace EnduranceJudge.Gateways.Desktop.Core.Static
{
    public interface IExplorerService : IService
    {
        string SelectDirectory();

        string SelectFile();
    }
}
