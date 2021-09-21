using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.SimpleListItem;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using System.Collections.ObjectModel;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.PhasesForCategory
{
    public class PhaseForCategoryViewModel : ChildFormBase<PhaseForCategoryView>, IPhaseForCategoryState
    {
        public ObservableCollection<SimpleListItemViewModel> CategoryItems { get; }
            = new(SimpleListItemViewModel.FromEnum<Category>());

        private int maxRecoveryTimeInMinutes;
        private int restTimeInMinutes;
        private int categoryId;

        public int MaxRecoveryTimeInMinutes
        {
            get => this.maxRecoveryTimeInMinutes;
            set => this.SetProperty(ref this.maxRecoveryTimeInMinutes, value);
        }
        public int RestTimeInMinutes
        {
            get => this.restTimeInMinutes;
            set => this.SetProperty(ref this.restTimeInMinutes, value);
        }
        public int CategoryId
        {
            get => this.categoryId;
            set => this.SetProperty(ref this.categoryId, value);
        }

        public string CategoryName => this.Category.ToString();
        public Category Category => (Category)this.CategoryId;
    }
}
