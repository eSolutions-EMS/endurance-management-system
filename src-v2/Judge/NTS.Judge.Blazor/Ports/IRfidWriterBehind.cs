using Not.Injection;
using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Blazor.Ports;

public interface IRfidWriterBehind : ITransientService
{
    Task<Tag> WriteTag(int number);
}
