using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Core.Models;
using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public abstract class ParentFormBase<TView> : FormBase<TView>, IParentForm
        where TView : IView
    {
        public override void OnNavigatedTo(NavigationContext context)
        {
            base.OnNavigatedTo(context);
            if (context.HasChild())
            {
                this.HandleChildren(context);
            }
        }

        public virtual void HandleChildren(NavigationContext context)
        {
        }

        public void UpdateGrandChild<T>(NavigationContext context, ObservableCollection<T> children)
            where T : IParentForm, IObject
        {
            context.Parameters.Add(DesktopConstants.UpdateOnlyParameter, true);
            children.HandleChildren(context);
        }

        protected void AddOrUpdateChild<T>(NavigationContext context, ObservableCollection<T> parents)
            where T : IParentForm, IObject
        {
            var child = context.GetChild<T>();
            if (child != null)
            {
                if (context.IsUpdateOnly())
                {
                    parents.UpdateObject(child);
                }
                else
                {
                    parents.AddOrUpdateObject(child);
                }
            }
        }

        protected void NavigateToNewChild<T>()
            where T : IView
        {
            var guid = Guid.NewGuid();
            var newChild = new NavigationParameter(DesktopConstants.NewChildIdParameter, guid);
            this.Navigation.ChangeTo<T>(newChild);
        }
    }
}
