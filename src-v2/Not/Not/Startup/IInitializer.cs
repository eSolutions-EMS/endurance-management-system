using Not.Injection;

namespace Not.Startup;

/// <summary>
/// Execute synchronous code at application startup. 
/// </summary>
public interface IInitializer
{
    void Run();
}