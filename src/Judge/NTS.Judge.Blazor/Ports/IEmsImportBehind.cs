using Not.Injection;

namespace NTS.Judge.Blazor.Ports;

public interface IEmsImportBehind : ITransientService
{
    Task Import(string emsStateFilePath);
    Task ImportCore(string path);
}
