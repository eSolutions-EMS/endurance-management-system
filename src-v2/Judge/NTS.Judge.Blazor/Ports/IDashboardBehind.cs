using Not.Injection;
namespace NTS.Judge.Blazor.Ports;

public interface IDashboardBehind : ITransientService
{
    Task Start();
}