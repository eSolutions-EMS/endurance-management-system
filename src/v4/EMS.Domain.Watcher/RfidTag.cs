using Core.Domain.Common.Models;

namespace EMS.Domain.Watcher;

public class RfidTag : DomainEntity
{
    public RfidTag(string id, string number, string position)
    {
        this.TagId = id;
        this.Number = number;
        this.Position = position;
    }

    public string TagId { get; set; }
    public string Number { get; set; }
    public string Position { get; set; }
}
