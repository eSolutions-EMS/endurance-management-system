using NTS.Domain.Objects;

namespace NTS.Judge.Blazor.Ports;
public interface ITagRead
{
    Task<IEnumerable<RfidSnapshot>> ReadTags();
}
