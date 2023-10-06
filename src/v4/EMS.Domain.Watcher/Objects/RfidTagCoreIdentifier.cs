using EMS.Domain.Watcher.Entities;

namespace EMS.Domain.Watcher.Objects;

public record RfidTagCoreIdentifier : CoreIdentifier
{
    public RfidTagCoreIdentifier(RfidTag tag)
    {
        Number = int.Parse(tag.Number);
    }
}
