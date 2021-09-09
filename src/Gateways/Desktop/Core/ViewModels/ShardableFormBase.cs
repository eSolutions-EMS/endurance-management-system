using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using Prism.Regions;
using System;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public abstract class ShardableFormBase<TView> : FormBase<TView>
        where TView : IView
    {
        public override void OnNavigatedTo(NavigationContext context)
        {
            base.OnNavigatedTo(context);
            if (context.HasDependant())
            {
                this.HandleChildren(context);
            }
        }

        protected virtual void HandleChildren(NavigationContext context)
        {
        }

        protected void NavigateToDependantCreate<T>()
            where T : IView
        {
            var guid = Guid.NewGuid();
            var newDependant = new NavigationParameter(DesktopConstants.NewDependantId, guid);
            this.Navigation.ChangeTo<T>(newDependant);
        }

        protected void NavigateToDependantUpdate<T>(object data)
            where T : IView
        {
            var dependant = new NavigationParameter(DesktopConstants.DependantDataParameter, data);
            this.Navigation.ChangeTo<T>(dependant);
        }
    }
}
