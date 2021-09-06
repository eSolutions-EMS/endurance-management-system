using EnduranceJudge.Application.Events.Queries.GetEvent;
using EnduranceJudge.Core.Models;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.EnduranceEvents;
using Prism.Commands;

namespace EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem
{
    public class ListItemViewModel
    {
        private ListItemViewModel()
        {
        }

        public ListItemViewModel(int id, string name, DelegateCommandBase command)
        {
            this.Id = id;
            this.Name = name;
            this.Command = command;
        }

        public ListItemViewModel(IListable item, DelegateCommandBase command) : this(item.Id, item.Name, command)
        {
        }

        public ListItemViewModel(int id, string name, object content, DelegateCommandBase command)
            : this(id, name, command)
        {
            this.Content = content;
        }

        public int Id { get; }
        public string Name { get; }
        public object Content { get; }
        public DelegateCommandBase Command { get; }
    }
}
