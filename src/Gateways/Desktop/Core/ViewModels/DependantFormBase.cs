using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Regions;
using System;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public abstract class DependantFormBase : ShardableFormBase, IDependantForm
    {
        private Action<object> submitAction;

        protected DependantFormBase(IApplicationService application, INavigationService navigation) : base(navigation)
        {
            this.Application = application;
            this.DependantId = Guid.NewGuid();
        }

        public Guid? DependantId { get; private set; }
        protected IApplicationService Application { get; }

        protected override void NavigateBackAction()
        {
            this.submitAction(this);
            base.NavigateBackAction();
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var dependantId = navigationContext.GetDependantId();
            if (dependantId != null)
            {
                return this.DependantId == dependantId;
            }
            var data = navigationContext.GetData();
            if (data != null)
            {
                return this.Equals(data);
            }

            return false;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var dependantId = navigationContext.GetDependantId();
            if (this.DependantId == null && dependantId != null)
            {
                this.DependantId = dependantId;
            }
            var data = navigationContext.GetData();
            if (this.Id == default && data != null)
            {
                this.MapFrom(data);
            }

            this.submitAction = navigationContext.GetSubmitAction();

            base.OnNavigatedTo(navigationContext);
        }

        public override int GetHashCode()
        {
            // Does not call base.GetHashCode on purpose, since navigation in nested forms happens with 'DependantId'.
            // It is not readonly, because we have to provide the 'DependantId' as NavigationParameter when adding
            // the dependant entity. This is necessary, because Prism's Journal keeps only RequestNavigation calls
            // in the back steps list, rather than some sort of view ID. Hence if we want to be able to back-up to the
            // same newly created view instance we have to have provided ID in the first place. Otherwise Prism will
            // simply create new instance, causing the infamous back-up problem.
            return (this.GetType().ToString() + this.DependantId).GetHashCode();
        }
    }
}
