using EMS.Judge.Common;
using EMS.Judge.Application.Common;
using Core.Domain.Core.Models;
using EMS.Judge.Common.Extensions;
using Prism.Regions;
using System;

namespace EMS.Judge.Views.Content.Configuration.Core;

public abstract class NestedConfigurationBase<TView, TDomain> : ConfigurationBase<TView, TDomain>
    where TView : IView
    where TDomain : IDomain
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
        where T : IView => this.Executor.Execute(() =>
    {
        this.Persist();
        var childViewId = Random.Next();
        this.Navigation.ChangeToNewConfiguration<T>(this.Id, childViewId);
    }, true);
}
