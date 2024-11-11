using Not.Injection;
using NTS.Domain.Setup.Import;

namespace NTS.Domain.Setup.Ports;

public interface IParser<T> : ITransient
    where T : IImportable
{
    IEnumerable<T> Parse(string contents);
}
