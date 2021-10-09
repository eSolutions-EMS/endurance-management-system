using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.SimpleListItem;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using System.Collections.Generic;
using ServiceProvider = EnduranceJudge.Gateways.Desktop.Core.Static.ServiceProvider;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public abstract class FormBase<TView> : ViewModelBase
        where TView : IView
    {

        protected FormBase()
        {
            this.Navigation = ServiceProvider.GetService<INavigationService>();
            this.BoolItems = SimpleListItemViewModel.FromBool();
            this.NavigateToUpdate = new DelegateCommand(
                () => Navigation.ChangeTo<TView>(new NavigationParameter(DesktopConstants.DataParameter, this)));
        }

        protected INavigationService Navigation { get; }

        private readonly int id;

        protected abstract void Load(int id);
        protected abstract void SubmitAction();
        public DelegateCommand Submit { get; }
        public DelegateCommand NavigateToUpdate { get; }

        public override void OnNavigatedTo(Prism.Regions.NavigationContext context)
        {
            this.Load(default);
            base.OnNavigatedTo(context);
        }

        public List<SimpleListItemViewModel> BoolItems { get; }

        public int Id
        {
            get => this.id;
            init => this.SetProperty(ref this.id, value);
        }
    }
}
