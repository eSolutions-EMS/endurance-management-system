using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.SimpleListItem;
using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public abstract class FormBase<TView> : ViewModelBase
        where TView : IView
    {
        private readonly IDomainHandler domainHandler;
        private readonly IPersistence persistence;
        protected INavigationService Navigation { get; }
        protected int? PrincipalId { get; private set; }

        protected FormBase()
        {
            this.Navigation = StaticProvider.GetService<INavigationService>();
            this.domainHandler = StaticProvider.GetService<IDomainHandler>();
            this.persistence = StaticProvider.GetService<IPersistence>();

            this.BoolItems = SimpleListItemViewModel.FromBool();

            this.Submit = new DelegateCommand(this.SubmitAction);
            this.NavigateToUpdate = new DelegateCommand(() => Navigation.ChangeToUpdateForm<TView>(this.Id));
        }

        public DelegateCommand Submit { get; }
        public DelegateCommand NavigateToUpdate { get; }

        private readonly int id;
        public List<SimpleListItemViewModel> BoolItems { get; }

        protected abstract void Load(int id);
        protected abstract void DomainAction();

        public override void OnNavigatedTo(Prism.Regions.NavigationContext context)
        {
            var id = context.GetId();
            if (id.HasValue)
            {
                this.Load(id.Value);
            }
            else
            {
                this.PrincipalId = context.GetPrincipalId();
            }
            base.OnNavigatedTo(context);
        }

        private void SubmitAction() => this.domainHandler.Handle(() =>
        {
            this.DomainAction();
            this.persistence.Snapshot();
            this.NavigateBackAction();
        });

        public int Id
        {
            get => this.id;
            init => this.SetProperty(ref this.id, value);
        }
    }
}
