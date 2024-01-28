using EMS.Domain.Objects;
using System.ComponentModel.DataAnnotations;

namespace EMS.Judge.Setup.Events;

public interface IEventForm
{
    string? Place { get; set; }
    Country? Country { get; set; }
}
