using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Application.Core.Models;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Configuration;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Roots.Horses.Listing
{
    public class HorseListViewModel : SearchableListViewModelBase<HorseView>
    {
        private readonly IDomainExecutor<ConfigurationManager> domainExecutor;
        private readonly IQueries<Horse> horses;

        public HorseListViewModel(
            IDomainExecutor<ConfigurationManager> domainExecutor,
            IQueries<Horse> horses,
            IPersistence persistence,
            INavigationService navigation) : base(navigation, domainExecutor, persistence)
        {
            this.domainExecutor = domainExecutor;
            this.horses = horses;
        }

        protected override IEnumerable<ListItemModel> LoadData()
        {
            var horses = this.horses
                .GetAll()
                .MapEnumerable<ListItemModel>();
            return horses;
        }
        protected override void RemoveDomain(int id)
        {
            var configurations = new ConfigurationManager();
            configurations.Horses.Remove(id);
        }
    }
}
