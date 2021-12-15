using Prism.Commands;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem
{
    public class ListItemViewModel
    {
        private ListItemViewModel()
        {
        }

        public ListItemViewModel(int id, DelegateCommandBase command, string actionName)
            : this(id, id.ToString(), command, actionName)
        {
        }

        public ListItemViewModel(int id, string name, DelegateCommandBase action, string actionName)
            : this(id, name, action, actionName, null)
        {
        }

        public ListItemViewModel(
            int id,
            string name,
            DelegateCommandBase action,
            string actionName,
            DelegateCommandBase remove)
        {
            this.Id = id;
            this.Name = name;
            this.ActionName = actionName;
            this.Action = action;
            this.Remove = remove;
        }

        public int Id { get; }
        public string Name { get; }
        public string ActionName { get; }
        public DelegateCommandBase Action { get; }
        public DelegateCommandBase Remove { get; }
        public Visibility RemoveVisibility => this.Remove != null
            ? Visibility.Visible
            : Visibility.Collapsed;
    }
}
