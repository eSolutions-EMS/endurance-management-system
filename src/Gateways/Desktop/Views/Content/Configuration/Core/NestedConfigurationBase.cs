using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using Prism.Regions;
using System;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Core
{
    public abstract class NestedConfigurationBase<TView, TDomain> : ConfigurationBase<TView, TDomain>
        where TView : IView
        where TDomain : IDomainObject
    {
        private static readonly Random Random = new ();
        protected NestedConfigurationBase(IQueries<TDomain> queries) : base(queries)
        {
        }

        protected int? ParentId { get; private set; }
        protected int ViewId { get; private set; }

        public override bool IsNavigationTarget(NavigationContext context)
        {
            if (context.IsExistingConfiguration())
            {
                return false;
            }
            else
            {
                var viewId = context.GetViewId();
                return this.ViewId == viewId;
            }
        }

        public override void OnNavigatedTo(NavigationContext context)
        {
            if (this.ViewId != default)
            {
                this.Load(this.Id);
            }
            else if (context.IsExistingConfiguration())
            {
                var id = context.GetDomainId();
                this.Load(id);
            }
            else
            {
                this.ViewId = context.GetViewId();
                this.ParentId = context.LookForParentViewId();
            }
            base.OnNavigatedTo(context);
        }

        protected void NewForm<T>()
            where T : IView => this.BasicExecutor.Execute(() =>
        {
            this.Persist();
            var childViewId = Random.Next();
            this.Navigation.ChangeToNewConfiguration<T>(this.Id, childViewId);
        });
    }
}
