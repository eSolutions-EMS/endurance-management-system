using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using Prism.Commands;
using Prism.Regions;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public abstract class RootFormBase<TView> : ParentFormBase<TView>
        where TView : IView
    {
        protected RootFormBase()
        {
            this.Submit = new DelegateCommand(this.SubmitAction);
        }

        public DelegateCommand Submit { get; }

        public override bool IsNavigationTarget(NavigationContext context)
        {
            if (this.IsCreateForm)
            {
                return true;
            }

            var id = context.GetId();
            return this.Id == id;
        }

        public override void OnNavigatedTo(NavigationContext context)
        {
            this.Load(default);
            base.OnNavigatedTo(context);
        }

        protected abstract void Load(int id);
        protected abstract void SubmitAction();

        private bool IsCreateForm => this.Id == default;
    }
}
