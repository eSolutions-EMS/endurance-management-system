namespace EMS.Domain.Watcher;

public class RfidTagCoreIdentifier : CoreIdentifier
{
    public RfidTagCoreIdentifier(RfidTag tag)
    {
        this.Number = int.Parse(tag.Number);
    }
}
