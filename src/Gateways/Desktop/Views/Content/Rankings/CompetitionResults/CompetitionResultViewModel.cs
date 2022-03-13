using EnduranceJudge.Domain.AggregateRoots.Rankings;
using EnduranceJudge.Domain.AggregateRoots.Rankings.Aggregates;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Rankings.RankLists;
using Prism.Commands;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Rankings.CompetitionResults;

public class CompetitionResultViewModel : ViewModelBase
{
    private readonly Executor<RankingRoot> rankingExecutor;
    private CompetitionResultAggregate selectedCompetition;
    private List<CompetitionResultAggregate> competitions;

    public CompetitionResultViewModel(IPrinter printer, Executor<RankingRoot> rankingExecutor)
    {
        this.rankingExecutor = rankingExecutor;
        this.Print = new DelegateCommand<Visual>(printer.Print);
        this.SelectKidsCategory = new DelegateCommand(this.SelectKidsCategoryAction);
        this.SelectAdultsCategory = new DelegateCommand(this.SelectAdultsCategoryAction);
        this.SelectCompetition = new DelegateCommand<int?>(x => this.SelectCompetitionAction(x!.Value));
    }

    public DelegateCommand<int?> SelectCompetition { get; }
    public DelegateCommand<Visual> Print { get; }
    public DelegateCommand SelectKidsCategory { get; }
    public DelegateCommand SelectAdultsCategory { get; }

    // This should not be a collection and should always have only a single instance
    // It is defined as collection in order to work-around
    // my inability to render a template outside of a list.
    public ObservableCollection<RankListTemplateModel> RankList { get; } = new();
    public ObservableCollection<ListItemViewModel> Competitions { get; } = new();
    private string totalLengthInKm;
    private string categoryName;
    private bool hasKidsClassification;
    private bool hasAdultsClassification;


    public override void OnNavigatedTo(NavigationContext context)
    {
        this.competitions = this.rankingExecutor
            .Execute(ranking => ranking.Competitions)
            .ToList();
        if (this.competitions.Count != 0)
        {
            foreach (var competition in competitions)
            {
                var viewModel = this.ToListItem(competition);
                this.Competitions.Add(viewModel);
            }
            this.SelectCompetitionAction(this.competitions[0].Id);
        }
        base.OnNavigatedTo(context);
    }

    private void SelectCompetitionAction(int competitionId)
    {
        var competition = this.rankingExecutor.Execute(ranking => ranking.GetCompetition(competitionId));
        this.selectedCompetition = competition;
        this.HasAdultsClassification = competition.AdultsRankList != null;
        this.HasKidsClassification = competition.KidsRankList != null;
        this.SelectDefault();
    }

    private ListItemViewModel ToListItem(CompetitionResultAggregate resultAggregate)
    {
        var command = new DelegateCommand<int?>(x => this.SelectCompetitionAction(x!.Value));
        var listItem = new ListItemViewModel(resultAggregate.Id, resultAggregate.CompetitionName, command, VIEW);
        return listItem;
    }

    private void SelectKidsCategoryAction()
    {
        this.Select(this.selectedCompetition.KidsRankList);
    }
    private void SelectAdultsCategoryAction()
    {
        this.Select(this.selectedCompetition.AdultsRankList);
    }
    private void SelectDefault()
    {
        var rankList = this.selectedCompetition.AdultsRankList
            ?? this.selectedCompetition.KidsRankList;
        this.Select(rankList);
    }
    private void Select(RankList rankList)
    {
        this.RankList.Clear();
        var template = new RankListTemplateModel(rankList, this.selectedCompetition);
        this.RankList.Add(template);
        this.CategoryName = rankList.Category.ToString();
    }

    #region Setters
    public string TotalLengthInKm
    {
        get => this.totalLengthInKm;
        set => this.SetProperty(ref this.totalLengthInKm, value);
    }
    public string CategoryName
    {
        get => this.categoryName;
        set => this.SetProperty(ref this.categoryName, value);
    }
    public bool HasKidsClassification
    {
        get => this.hasKidsClassification;
        set => this.SetProperty(ref this.hasKidsClassification, value);
    }
    public bool HasAdultsClassification
    {
        get => this.hasAdultsClassification;
        set => this.SetProperty(ref this.hasAdultsClassification, value);
    }
    #endregion
}
