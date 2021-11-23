using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.SimpleListItem;
using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using Prism.Regions;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Core
{
    public abstract class ConfigurationBase<TView, TDomain> : ViewModelBase
        where TView : IView
        where TDomain : IDomainObject
    {
        private readonly IQueries<TDomain> queries;
        protected INavigationService Navigation { get; }
        protected IBasicExecutor Executor { get; }

        protected ConfigurationBase(IQueries<TDomain> queries)
        {
            this.queries = queries;
            this.Navigation = StaticProvider.GetService<INavigationService>();
            this.Executor = StaticProvider.GetService<IBasicExecutor>();

            this.BoolItems = SimpleListItemViewModel.FromBool();

            this.Submit = new DelegateCommand(this.SubmitAction);
            this.NavigateToUpdate = new DelegateCommand(() => Navigation.ChangeToUpdateConfiguration<TView>(this.Id));
        }

        public DelegateCommand Submit { get; }
        public DelegateCommand NavigateToUpdate { get; }

        private readonly int id;
        public List<SimpleListItemViewModel> BoolItems { get; }

        protected abstract IDomainObject Persist();

        public override void OnNavigatedTo(NavigationContext context)
        {
            if (context.IsExistingConfiguration())
            {
                var id = context.GetDomainId();
                this.Load(id);
            }
            base.OnNavigatedTo(context);
        }

        protected virtual void Load(int id)
        {
            var domainObject = this.queries.GetOne(id);
            this.MapFrom(domainObject);
        }
        private void SubmitAction() => this.Executor.Execute(() =>
        {
            this.Persist();
            this.NavigateBackAction();
        });

        public int Id
        {
            get => this.id;
            init => this.SetProperty(ref this.id, value);
        }
    }
}
