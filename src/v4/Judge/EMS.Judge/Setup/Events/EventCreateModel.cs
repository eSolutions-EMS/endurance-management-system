using EMS.Domain.Objects;
using System.ComponentModel.DataAnnotations;

namespace EMS.Judge.Setup.Events;

public class EventCreateModel
{
    [Required]
    public string? Place { get; set; }
    [Required]
    public Country? Country { get; set; }
}