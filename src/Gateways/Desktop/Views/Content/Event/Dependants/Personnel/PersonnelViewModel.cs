using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.SimpleListItem;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using System.Collections.ObjectModel;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Personnel
{
    public class PersonnelViewModel : DependantFormBase<PersonnelView>, IMap<PersonnelDependantModel>
    {
        public ObservableCollection<SimpleListItemViewModel> RoleItems { get; private set; }

        private string name;
        private int role;

        public string Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }
        public int Role
        {
            get => this.role;
            set => this.SetProperty(ref this.role, value);
        }

        public string RoleName => this.RoleItems[this.Role].Name;

        protected override void Initialize()
        {
            this.LoadRoles();
        }

        private void LoadRoles()
        {
            var roles = SimpleListItemViewModel.FromEnum<PersonnelRole>();
            this.RoleItems = new ObservableCollection<SimpleListItemViewModel>(roles);
        }
    }
}
