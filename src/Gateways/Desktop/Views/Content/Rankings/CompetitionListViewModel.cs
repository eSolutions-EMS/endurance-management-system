using EnduranceJudge.Domain.AggregateRoots.Rankings;
using EnduranceJudge.Domain.AggregateRoots.Rankings.Aggregates;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Rankings.CompetitionResults;
using static EnduranceJudge.Localization.Strings;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using static EnduranceJudge.Gateways.Desktop.DesktopConstants;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Rankings;

public class CompetitionListViewModel : ViewModelBase
{
    private readonly INavigationService navigation;
    private readonly RankingRoot rankingRoot;

    public CompetitionListViewModel(INavigationService navigation, RankingRoot rankingRoot)
    {
        this.navigation = navigation;
        this.rankingRoot = rankingRoot;
    }



    private void NavigateToClassification(int? id)
    {
        var categorization = this.rankingRoot.Competitions.First(x => x.Id == id!.Value);
        var dataParameter = new NavigationParameter(NavigationParametersKeys.DATA, categorization);
        this.navigation.ChangeTo<CompetitionResultView>(dataParameter);
    }
}
