using EnduranceJudge.Application.Models;

namespace EnduranceJudge.Application.Services;

public interface IStorageInitializer
{
    PersistenceResult Initialize(string directoryPath);
}
