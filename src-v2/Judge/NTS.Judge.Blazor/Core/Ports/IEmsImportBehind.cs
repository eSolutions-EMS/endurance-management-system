using Not.Injection;

namespace NTS.Judge.Blazor.Core.Ports;

public interface IEmsImportBehind : ITransient
{
    Task Import(string emsStateFilePath);
    Task ImportCore(string path);
}
