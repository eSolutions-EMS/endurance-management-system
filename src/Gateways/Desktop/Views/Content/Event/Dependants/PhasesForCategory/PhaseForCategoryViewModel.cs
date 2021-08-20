using EnduranceJudge.Core.Models;
using EnduranceJudge.Domain.Aggregates.Event.PhasesForCategory;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ComboBoxItem;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using System.Collections.ObjectModel;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.PhasesForCategory
{
    public class PhaseForCategoryViewModel : DependantFormBase,
        IPhaseForCategoryState,
        IListable
    {
        private PhaseForCategoryViewModel() : base(null, null)
        {
        }

        public PhaseForCategoryViewModel(IApplicationService application, INavigationService navigation)
            : base(application, navigation)
        {
        }

        public ObservableCollection<ComboBoxItemViewModel> CategoryItems { get; }
            = new(ComboBoxItemViewModel.FromEnum<Category>());

        private int maxRecoveryTimeInMinutes;
        public int MaxRecoveryTimeInMinutes
        {
            get => this.maxRecoveryTimeInMinutes;
            set => this.SetProperty(ref this.maxRecoveryTimeInMinutes, value);
        }

        private int restTimeInMinutes;
        public int RestTimeInMinutes
        {
            get => this.restTimeInMinutes;
            set => this.SetProperty(ref this.restTimeInMinutes, value);
        }

        private int categoryId;
        public int CategoryId
        {
            get => this.categoryId;
            set => this.SetProperty(ref this.categoryId, value);
        }

        protected override ListItemViewModel ToListItem(DelegateCommand command)
            => new(this, command);

        public string Name => this.Category.ToString();
        public Category Category => (Category)this.CategoryId;
    }
}
