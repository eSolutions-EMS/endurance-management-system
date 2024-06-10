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
using System.IO;
using EMS.Judge.Common.Services;

namespace EMS.Judge.Views.Content.Ranking;

public class RankingViewModel : ViewModelBase
{
    private readonly IFileService _fileService;
    private readonly IExecutor<RankingRoot> _rankingExecutor;
    private readonly IExecutor basicExecutor;
    private readonly IXmlSerializationService _xmlSerializationService;
    private readonly IPopupService _popupService;
    private CompetitionResultAggregate _selectedCompetition;
    private List<CompetitionResultAggregate> competitions;

    public RankingViewModel(
        IFileService fileService,
        IExecutor<RankingRoot> rankingExecutor,
        IExecutor basicExecutor,
        IXmlSerializationService xmlSerializationService,
        IPopupService popupService)
    {
        _fileService = fileService;
        this._rankingExecutor = rankingExecutor;
        this.basicExecutor = basicExecutor;
        _xmlSerializationService = xmlSerializationService;
        _popupService = popupService;
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
        this._selectedCompetition = competition;

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
            rankingRoot => {
                var contents = rankingRoot.GenerateFeiExport(_selectedCompetition.Id);
                var path = $"{ManagerRoot.dataDirectoryPath}/{_selectedCompetition.Name}.xml";
                _fileService.Create(path, contents);
                _popupService.RenderOk();
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
        var printer = new RanklistPrinter(this._selectedCompetition.Name, control.Ranklist, this.CategoryName);
            printer.PreviewDocument();
        }, false);
    }
    private void SelectCategory(Category category)
    {
        this.Ranklist = this._selectedCompetition.Rank(category);
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
