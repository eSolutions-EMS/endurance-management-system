using Not.Injection;
using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Blazor.Setup.Ports;

public interface IRfidWriterBehind : ITransient
{
    Task<Tag> WriteTag(int number);
}
