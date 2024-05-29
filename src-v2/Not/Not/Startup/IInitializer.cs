using Not.Injection;

namespace Not.Startup;

public interface IInitializer : ISingletonService
{
    void Run();
}
