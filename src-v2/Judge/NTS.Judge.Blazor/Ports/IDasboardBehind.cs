using Not.Injection;
namespace NTS.Judge.Blazor.Ports;

public interface IDasboardBehind : ITransientService
{
    Task Start();
}