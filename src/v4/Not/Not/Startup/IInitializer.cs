using Not.Conventions;

namespace Not.Startup;

public interface IInitializer : ITransientService
{
    void Run();
}
