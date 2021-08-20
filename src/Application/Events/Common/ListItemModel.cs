using EnduranceJudge.Core.Models;

namespace EnduranceJudge.Application.Events.Common
{
    public class ListItemModel : IListable
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
