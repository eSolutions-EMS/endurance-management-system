using EnduranceJudge.Domain.Aggregates.Rankings.AggregateBranches;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Rankings.RankLists;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Rankings.CompetitionResults
{
    public class CompetitionResultViewModel : ViewModelBase
    {
        private CompetitionResult result;

        public CompetitionResultViewModel(IPrinter printer)
        {
            this.Print = new DelegateCommand<Visual>(printer.Print);
            this.SelectKidsCategory = new DelegateCommand(this.SelectKidsCategoryAction);
            this.SelectAdultsCategory = new DelegateCommand(this.SelectAdultsCategoryAction);
        }

        public DelegateCommand<Visual> Print { get; }
        public DelegateCommand SelectKidsCategory { get; }
        public DelegateCommand SelectAdultsCategory { get; }

        // This should not be a collection and should always have only a single instance
        // It is defined as collection in order to work-around
        // my inability to render a template outside of a list.
        public ObservableCollection<RankListTemplateModel> RankList { get; } = new();
        private string totalLengthInKm;
        private string categoryName;
        private bool hasKidsClassification;
        private bool hasAdultsClassification;

        public override void OnNavigatedTo(NavigationContext context)
        {
            base.OnNavigatedTo(context);
            var data = context.GetData();
            if (data is not CompetitionResult categorization)
            {
                throw new InvalidOperationException(
                    $"Data is of type '{data.GetType()}'. Expected type is 'Classification'");
            }
            this.result = categorization;

            this.HasAdultsClassification = categorization.AdultsRankList != null;
            this.HasKidsClassification = categorization.KidsRankList != null;
            this.SelectDefault();
        }

        private void SelectKidsCategoryAction()
        {
            this.Select(this.result.KidsRankList);
        }
        private void SelectAdultsCategoryAction()
        {
            this.Select(this.result.AdultsRankList);
        }
        private void SelectDefault()
        {
            var rankList = this.result.AdultsRankList
                ?? this.result.KidsRankList;
            this.Select(rankList);
        }
        private void Select(RankList rankList)
        {
            this.RankList.Clear();
            var template = new RankListTemplateModel(rankList, this.result);
            this.RankList.Add(template);
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
}
