using Not.Injection;
using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Blazor.Ports;
public interface ITagWriter : ITransientService
{
    Task<IdTag> WriteTag(int number);
}
