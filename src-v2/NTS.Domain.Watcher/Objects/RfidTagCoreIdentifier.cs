using NTS.Domain.Watcher.Entities;

namespace NTS.Domain.Watcher.Objects;

public record RfidTagCoreIdentifier : CoreIdentifier
{
    public RfidTagCoreIdentifier(RfidTag tag)
    {
        Number = int.Parse(tag.Number);
    }
}
