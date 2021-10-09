using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Configuration;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.State.Personnels;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.SimpleListItem;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using System.Collections.ObjectModel;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.Personnel
{
    public class PersonnelViewModel : FormBase<PersonnelView>, IPersonnelState, IMap<Domain.State.Personnels.Personnel>
    {
        private readonly IQueries<Domain.State.Personnels.Personnel> personnel;
        private PersonnelViewModel() {}
        public PersonnelViewModel(IQueries<Domain.State.Personnels.Personnel> personnel)
        {
            this.personnel = personnel;
        }

        public ObservableCollection<SimpleListItemViewModel> RoleItems { get; }
            = new(SimpleListItemViewModel.FromEnum<PersonnelRole>());

        private string name;
        private int roleId;

        protected override void Load(int id)
        {
            var personnel = this.personnel.GetOne(id);
            this.MapFrom(personnel);
        }
        protected override void DomainAction()
        {
            var configuration = new ConfigurationManager();
            configuration.Save(this);
        }

        public string Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }
        public int RoleId
        {
            get => this.roleId;
            set => this.SetProperty(ref this.roleId, value);
        }

        public PersonnelRole Role => (PersonnelRole)this.RoleId;
        public string RoleName => this.RoleItems[this.RoleId].Name;
    }
}
