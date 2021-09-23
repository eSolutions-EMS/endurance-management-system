using EnduranceJudge.Application.Actions.Ranking.Queries.GetCompetitionList;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Domain.Aggregates.Rankings.Stateless.Rankings;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using Prism.Regions;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Rankings
{
    public class RankingViewModel : ViewModelBase
    {
        private readonly IApplicationService application;
        private Ranking ranking;

        public RankingViewModel(IApplicationService application)
        {
            this.application = application;
        }

        public override void OnNavigatedTo(NavigationContext context)
        {
            base.OnNavigatedTo(context);
            this.Load();
        }

        private async Task Load()
        {
            var query = new GetCompetitionList();
            var competitions = await this.application.Execute(query);
            this.ranking = new Ranking(competitions);
        }
    }
}
