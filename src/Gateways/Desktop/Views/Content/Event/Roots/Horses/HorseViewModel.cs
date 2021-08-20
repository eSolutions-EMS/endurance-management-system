using EnduranceJudge.Application.Events.Commands;
using EnduranceJudge.Application.Events.Models;
using EnduranceJudge.Application.Events.Queries.GetHorse;
using EnduranceJudge.Core.Models;
using EnduranceJudge.Domain.Aggregates.Common.Horses;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using MediatR;
using Prism.Commands;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.Horses
{
    public class HorseViewModel : RootFormBase<UpdateHorse, HorseRootModel>,
        IHorseState,
        IListable
    {
        public HorseViewModel(IApplicationService application, INavigationService navigation)
            : base(application, navigation)
        {
        }

        protected override ListItemViewModel ToListItem(DelegateCommand command)
            => new(this, command);
        protected override IRequest<HorseRootModel> LoadCommand(int id)
            => new GetHorse { Id = id };

        private int isStallionValue;
        private string feiId;
        private string name;
        private string breed;
        private string trainerFeiId;
        private string trainerFirstName;
        private string trainerLastName;

        public string FeiId
        {
            get => this.feiId;
            set => this.SetProperty(ref this.feiId, value);
        }
        public string Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }
        public int IsStallionValue
        {
            get => this.isStallionValue;
            set => this.SetProperty(ref this.isStallionValue, value);
        }
        public string Breed
        {
            get => this.breed;
            set => this.SetProperty(ref this.breed, value);
        }
        public string TrainerFeiId
        {
            get => this.trainerFeiId;
            set => this.SetProperty(ref this.trainerFeiId, value);
        }
        public string TrainerFirstName
        {
            get => this.trainerFirstName;
            set => this.SetProperty(ref this.trainerFirstName, value);
        }
        public string TrainerLastName
        {
            get => this.trainerLastName;
            set => this.SetProperty(ref this.trainerLastName, value);
        }

        public bool IsStallion => this.isStallionValue != 0;
    }
}
