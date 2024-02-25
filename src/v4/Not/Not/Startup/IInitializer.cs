using Common.Conventions;

namespace Common.Startup;

public interface IInitializer : ITransientService
{
    void Run();
}
