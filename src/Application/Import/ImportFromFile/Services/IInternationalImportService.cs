using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Aggregates.Import.EnduranceEvents;

namespace EnduranceJudge.Application.Import.ImportFromFile.Services
{
    public interface IInternationalImportService : IService
    {
        EnduranceEvent FromInternational(string filePath);
    }
}
