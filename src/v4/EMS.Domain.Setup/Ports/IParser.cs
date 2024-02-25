using Not.Conventions;
using EMS.Domain.Setup.Import;

namespace EMS.Domain.Setup.Ports;

public interface IParser<T> : ITransientService
    where T : IImportable
{
    IEnumerable<T> Parse(string contents);
}
