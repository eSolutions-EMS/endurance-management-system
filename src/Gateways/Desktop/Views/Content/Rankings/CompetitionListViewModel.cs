using EnduranceJudge.Domain.Aggregates.Rankings;
using EnduranceJudge.Domain.Aggregates.Rankings.AggregateBranches;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Rankings.CompetitionResults;
using EnduranceJudge.Localization.Translations;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using static EnduranceJudge.Gateways.Desktop.DesktopConstants;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Rankings
{
    public class CompetitionListViewModel : ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly RankingRoot rankingRoot;

        public CompetitionListViewModel(INavigationService navigation, RankingRoot rankingRoot)
        {
            this.navigation = navigation;
            this.rankingRoot = rankingRoot;
        }

        public ObservableCollection<ListItemViewModel> Competitions { get; } = new();

        public override void OnNavigatedTo(NavigationContext context)
        {
            foreach (var competition in this.rankingRoot.Competitions)
            {
                var viewModel = this.ToListItem(competition);
                this.Competitions.Add(viewModel);
            }
            base.OnNavigatedTo(context);
        }

        private ListItemViewModel ToListItem(CompetitionResultAggregate resultAggregate)
        {
            var command = new DelegateCommand<int?>(this.NavigateToClassification);
            var listItem = new ListItemViewModel(resultAggregate.Id, resultAggregate.CompetitionName, command, Words.VIEW);
            return listItem;
        }

        private void NavigateToClassification(int? id)
        {
            var categorization = this.rankingRoot.Competitions.First(x => x.Id == id!.Value);
            var dataParameter = new NavigationParameter(NavigationParametersKeys.DATA, categorization);
            this.navigation.ChangeTo<CompetitionResultView>(dataParameter);
        }
    }
}
