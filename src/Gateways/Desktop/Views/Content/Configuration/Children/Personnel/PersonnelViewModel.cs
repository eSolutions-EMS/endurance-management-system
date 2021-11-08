using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Domain.Aggregates.Configuration;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.State.Personnels;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.SimpleListItem;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Core;
using System.Collections.ObjectModel;
using System.Linq;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Personnel
{
    public class PersonnelViewModel : RelatedConfigurationBase<PersonnelView, Domain.State.Personnels.Personnel>,
        IPersonnelState
    {
        private readonly IDomainExecutor<ConfigurationManager> domainExecutor;
        private PersonnelViewModel() : this(null, null) {}
        public PersonnelViewModel(
            IDomainExecutor<ConfigurationManager> domainExecutor,
            IQueries<Domain.State.Personnels.Personnel> personnel) : base(personnel)
        {
            this.domainExecutor = domainExecutor;
        }

        public ObservableCollection<SimpleListItemViewModel> RoleItems { get; }
            = new(SimpleListItemViewModel.FromEnum<PersonnelRole>());

        private string name;
        private int roleId;

        protected override IDomainObject ActOnSubmit()
        {
            var result = this.domainExecutor.Write(x =>
                x.Save(this));
            return result;
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
        public string RoleName => this.RoleItems.First(x => x.Id == this.RoleId).Name;
    }
}
