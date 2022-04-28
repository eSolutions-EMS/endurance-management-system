using EnduranceJudge.Domain.AggregateRoots.Ranking;
using EnduranceJudge.Domain.AggregateRoots.Ranking.Aggregates;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Print.Performances;
using EnduranceJudge.Gateways.Desktop.Controls.Ranking;
using Prism.Commands;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Ranking;

public class RankingViewModel : ViewModelBase
{
    private readonly IExecutor<RankingRoot> rankingExecutor;
    private readonly IExecutor basicExecutor;
    private CompetitionResultAggregate selectedCompetition;
    private List<CompetitionResultAggregate> competitions;

    public RankingViewModel(IExecutor<RankingRoot> rankingExecutor, IExecutor basicExecutor)
    {
        this.rankingExecutor = rankingExecutor;
        this.basicExecutor = basicExecutor;
        this.Print = new DelegateCommand<RanklistControl>(this.PrintAction);
        this.SelectKidsCategory = new DelegateCommand(this.SelectKidsCategoryAction);
        this.SelectAdultsCategory = new DelegateCommand(this.SelectAdultsCategoryAction);
        this.SelectCompetition = new DelegateCommand<int?>(x => this.SelectCompetitionAction(x!.Value));
    }

    public DelegateCommand<int?> SelectCompetition { get; }
    public DelegateCommand<RanklistControl> Print { get; }
    public DelegateCommand SelectKidsCategory { get; }
    public DelegateCommand SelectAdultsCategory { get; }
    public ObservableCollection<ListItemViewModel> Competitions { get; } = new();
    private string totalLengthInKm;
    private string categoryName;
    private bool hasKidsClassification;
    private bool hasAdultsClassification;
    private RanklistAggregate ranklist;

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
        // TODO: Select competition only if Event has started
        var competition = this.rankingExecutor.Execute(ranking => ranking.GetCompetition(competitionId));
        this.selectedCompetition = competition;
        this.HasAdultsClassification = competition.AdultsRanklist != null;
        this.HasKidsClassification = competition.KidsRanklist != null;

        this.SelectCategory(competition.DefaultRanklist);
    }

    private ListItemViewModel ToListItem(CompetitionResultAggregate resultAggregate)
    {
        var command = new DelegateCommand<int?>(x => this.SelectCompetitionAction(x!.Value));
        var listItem = new ListItemViewModel(resultAggregate.Id, resultAggregate.Name, command, VIEW);
        return listItem;
    }

    private void SelectKidsCategoryAction()
    {
        this.SelectCategory(this.selectedCompetition.KidsRanklist);
    }
    private void SelectAdultsCategoryAction()
    {
        this.SelectCategory(this.selectedCompetition.AdultsRanklist);
    }
    private void PrintAction(RanklistControl control)
    {
        this.basicExecutor.Execute(() =>
        {
            var printer = new RanklistPrinter(this.selectedCompetition.Name, control.Ranklist);
            printer.PreviewDocument();
        });
    }
    private void SelectCategory(RanklistAggregate rankList)
    {
        this.Ranklist = rankList;
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
    public RanklistAggregate Ranklist
    {
        get => this.ranklist;
        private set => this.SetProperty(ref this.ranklist, value);
    }
#endregion
}
