using DryIoc;
using EnduranceJudge.Domain.Aggregates.Rankings;
using EnduranceJudge.Domain.Aggregates.Rankings.AggregateBranches;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Rankings.CompetitionResults;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using static EnduranceJudge.Localization.DesktopStrings;
using static EnduranceJudge.Gateways.Desktop.DesktopConstants;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Rankings
{
    public class RankingViewModel : ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly Ranking ranking;

        public RankingViewModel(INavigationService navigation, Ranking ranking)
        {
            this.navigation = navigation;
            this.ranking = ranking;
        }

        public ObservableCollection<ListItemViewModel> Competitions { get; } = new();

        public override void OnNavigatedTo(NavigationContext context)
        {
            foreach (var competition in this.ranking.Competitions)
            {
                var viewModel = this.ToListItem(competition);
                this.Competitions.Add(viewModel);
            }
            base.OnNavigatedTo(context);
        }

        private ListItemViewModel ToListItem(CompetitionResult competitionResult)
        {
            var command = new DelegateCommand<int?>(this.NavigateToClassification);
            var listItem = new ListItemViewModel((int)competitionResult.Length, command, VIEW);
            return listItem;
        }

        private void NavigateToClassification(int? length)
        {
            var categorization = this.ranking.Competitions.First(x => x.Length == length!.Value);
            var dataParameter = new NavigationParameter(NavigationParametersKeys.DATA, categorization);
            this.navigation.ChangeTo<CompetitionResultView>(dataParameter);
        }
    }
}
