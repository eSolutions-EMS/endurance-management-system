using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Persistence.Contracts.WorkFile
{
    public interface IWorkFileUpdater
    {
        Task Snapshot();
    }
}
