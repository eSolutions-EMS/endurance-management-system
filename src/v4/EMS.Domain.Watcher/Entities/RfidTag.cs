using Common.Domain;

namespace EMS.Domain.Watcher.Entities;

public class RfidTag : DomainEntity
{
    public RfidTag(string id, string number, string position)
    {
        TagId = id;
        Number = number;
        Position = position;
    }

    public string TagId { get; set; }
    public string Number { get; set; }
    public string Position { get; set; }
}
