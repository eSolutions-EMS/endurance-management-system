using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Application.Core.Models;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Configuration;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Services.Implementations;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Roots.Athletes.Listing
{
    public class AthleteListViewModel : SearchableListViewModelBase<AthleteView>
    {
        private readonly IDomainExecutor<ConfigurationManager> domainExecutor;
        private readonly IQueries<Athlete> athletes;
        public AthleteListViewModel(
            IDomainExecutor<ConfigurationManager> domainExecutor,
            IPersistence persistence,
            IQueries<Athlete> athletes,
            INavigationService navigation) : base(navigation, domainExecutor, persistence)
        {
            this.domainExecutor = domainExecutor;
            this.athletes = athletes;
        }

        protected override IEnumerable<ListItemModel> LoadData()
        {
            var athletes = this.athletes
                .GetAll()
                .MapEnumerable<ListItemModel>();
            return athletes;
        }
        protected override void RemoveDomain(int id)
        {
            this.domainExecutor.Write(x =>
                x.Athletes.Remove(id));
        }
    }
}
