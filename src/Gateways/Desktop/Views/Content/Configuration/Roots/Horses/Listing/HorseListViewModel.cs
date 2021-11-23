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
        private readonly ConfigurationManager manager;
        private readonly IQueries<Horse> horses;

        public HorseListViewModel(
            ConfigurationManager manager,
            IQueries<Horse> horses,
            IPersistence persistence,
            INavigationService navigation) : base(navigation, persistence)
        {
            this.manager = manager;
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
            this.manager.Horses.Remove(id);
        }
    }
}
