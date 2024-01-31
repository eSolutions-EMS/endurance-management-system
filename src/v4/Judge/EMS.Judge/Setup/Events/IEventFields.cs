using EMS.Domain.Objects;

namespace EMS.Judge.Setup.Events;

public interface IEventFields
{
    string? Place { get; set; }
    Country? Country { get; set; }
}
