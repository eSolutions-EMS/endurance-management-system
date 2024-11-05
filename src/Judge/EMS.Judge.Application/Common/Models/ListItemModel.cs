using Core.Domain.State.Horses;
using Core.Domain.State.Participants;
using Core.Mappings;

namespace EMS.Judge.Application.Common.Models;

public class ListItemModel : IMapFrom<Horse>, IMapFrom<Participant>
{
    public int Id { get; set; }
    public string Name { get; set; }
}
