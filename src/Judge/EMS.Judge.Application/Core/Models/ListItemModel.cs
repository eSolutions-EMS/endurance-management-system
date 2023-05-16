using EMS.Core.Mappings;
using EMS.Core.Domain.State.Horses;
using EMS.Core.Domain.State.Participants;

namespace EMS.Judge.Application.Core.Models;

public class ListItemModel
    : IMapFrom<Horse>,
        IMapFrom<Participant>
{
    public int Id { get; set; }
    public string Name { get; set; }
}
