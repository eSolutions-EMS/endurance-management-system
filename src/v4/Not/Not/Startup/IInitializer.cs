using Not.Injection;

namespace Not.Startup;

public interface IInitializer : ITransientService
{
    void Run();
}
