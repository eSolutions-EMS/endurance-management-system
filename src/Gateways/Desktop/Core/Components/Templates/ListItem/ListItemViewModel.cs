using EnduranceJudge.Core.Models;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ComboBoxItem;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using Prism.Commands;

namespace EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem
{
    public class ListItemViewModel : ComboBoxItemViewModel
    {
        public ListItemViewModel()
        {
        }

        public ListItemViewModel(IListable item, DelegateCommandBase command) : base(item)
        {
            this.Command = command;
        }

        public ListItemViewModel(int id, string name, DelegateCommandBase command) : base(id, name)
        {
            this.Command = command;
        }

        public DelegateCommandBase Command { get; }
    }
}
