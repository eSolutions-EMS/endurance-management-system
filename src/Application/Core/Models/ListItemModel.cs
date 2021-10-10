using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.State.Horses;

namespace EnduranceJudge.Application.Core.Models
{
    public class ListItemModel : IMapFrom<Horse>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
