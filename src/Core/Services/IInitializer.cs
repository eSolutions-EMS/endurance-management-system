namespace Core.Services;

public interface IInitializer
{
    int RunningOrder { get; }
    void Run();
}
