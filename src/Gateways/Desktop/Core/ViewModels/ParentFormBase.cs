using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using Prism.Regions;
using System;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public abstract class ParentFormBase<TView> : FormBase<TView>
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

        protected virtual void HandleChildren(NavigationContext context)
        {
        }

        protected void NavigateToNewChild<T>()
            where T : IView
        {
            var guid = Guid.NewGuid();
            var newChild = new NavigationParameter(DesktopConstants.NewChildId, guid);
            this.Navigation.ChangeTo<T>(newChild);
        }
    }
}
