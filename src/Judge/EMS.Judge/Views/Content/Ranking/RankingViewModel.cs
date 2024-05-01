using EMS.Judge.Controls.Ranking;
using EMS.Judge.Common;
using EMS.Judge.Common.Components.Templates.ListItem;
using EMS.Judge.Print.Performances;
using EMS.Judge.Services;
using Core.Domain.AggregateRoots.Ranking;
using Core.Domain.AggregateRoots.Ranking.Aggregates;
using Core.Domain.Enums;
using Prism.Commands;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static Core.Localization.Strings;
using Core.Services;
using Core.Domain.AggregateRoots.Manager;
using System;

namespace EMS.Judge.Views.Content.Ranking;

public class RankingViewModel : ViewModelBase
{
    private readonly IExecutor<RankingRoot> _rankingExecutor;
    private readonly IExecutor basicExecutor;
    private readonly IXmlSerializationService _xmlSerializationService;
    private CompetitionResultAggregate selectedCompetition;
    private List<CompetitionResultAggregate> competitions;

    public RankingViewModel(IExecutor<RankingRoot> rankingExecutor, IExecutor basicExecutor, IXmlSerializationService xmlSerializationService)
    {
        this._rankingExecutor = rankingExecutor;
        this.basicExecutor = basicExecutor;
        _xmlSerializationService = xmlSerializationService;
        this.Print = new DelegateCommand<RanklistControl>(this.PrintAction);
        this.SelectKidsCategory = new DelegateCommand(this.SelectKidsCategoryAction);
        this.SelectAdultsCategory = new DelegateCommand(this.SelectAdultsCategoryAction);
        this.SelectJuniorsCategory = new DelegateCommand(this.SelectJuniorsCategoryAction);
        this.SelectCompetition = new DelegateCommand<int?>(x => this.SelectCompetitionAction(x!.Value));
        Export = new DelegateCommand(this.ExportAction);
    }

    public DelegateCommand<int?> SelectCompetition { get; }
    public DelegateCommand<RanklistControl> Print { get; }
    public DelegateCommand SelectKidsCategory { get; }
    public DelegateCommand SelectAdultsCategory { get; }
    public DelegateCommand SelectJuniorsCategory { get; }
    public DelegateCommand Export { get; }
    public ObservableCollection<ListItemViewModel> Competitions { get; } = new();
    private string totalLengthInKm;
    private string categoryName;
    private bool hasKidsClassification;
    private bool hasAdultsClassification;
    private RanklistAggregate ranklist;

    public override void OnNavigatedTo(NavigationContext context)
    {
        this.competitions = this._rankingExecutor
            .Execute(ranking => ranking.Competitions, false)
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
        var competition = this._rankingExecutor.Execute(
            ranking => ranking.GetCompetition(competitionId),
            false);
        this.selectedCompetition = competition;

        this.SelectAdultsCategoryAction();
    }

    private ListItemViewModel ToListItem(CompetitionResultAggregate resultAggregate)
    {
        var command = new DelegateCommand<int?>(x => this.SelectCompetitionAction(x!.Value));
        var listItem = new ListItemViewModel(resultAggregate.Id, resultAggregate.Name, command, VIEW);
        return listItem;
    }

    private void ExportAction()
    {
        _rankingExecutor.Execute(
            x => {
                var result = x.GenerateFeiExport();
                _xmlSerializationService.SerializeToFile(
                    result,
                    $"{ManagerRoot.dataDirectoryPath}/{DateTime.Now.ToString(DesktopConstants.DATE_ONLY_FORMAT)}_export.xml");
            }, 
            false);
    }

    private void SelectKidsCategoryAction()
    {
        this.SelectCategory(Category.Children);
    }
    private void SelectAdultsCategoryAction()
    {
        this.SelectCategory(Category.Seniors);
    }
    private void SelectJuniorsCategoryAction()
    {
        this.SelectCategory(Category.JuniorOrYoungAdults);
    }
    private void PrintAction(RanklistControl control)
    {
        this.basicExecutor.Execute(() =>
        {
        var printer = new RanklistPrinter(this.selectedCompetition.Name, control.Ranklist, this.CategoryName);
            printer.PreviewDocument();
        }, false);
    }
    private void SelectCategory(Category category)
    {
        this.Ranklist = this.selectedCompetition.Rank(category);
        this.CategoryName = category == Category.JuniorOrYoungAdults
            ? "J/YR"
            : category.ToString();
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
    public RanklistAggregate Ranklist
    {
        get => this.ranklist;
        private set => this.SetProperty(ref this.ranklist, value);
    }
#endregion
}
