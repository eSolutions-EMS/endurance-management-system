using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using Prism.Regions;
using System;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public abstract class DependantFormBase<TView> : ShardableFormBase<TView>, IDependantForm
        where TView : IView
    {
        private bool isBackingUp = false;

        protected DependantFormBase()
        {
            this.DependantId = Guid.NewGuid();
        }

        public Guid? DependantId { get; private set; }

        protected override void NavigateBackAction()
        {
            this.isBackingUp = true;
            base.NavigateBackAction();
        }

        public override bool IsNavigationTarget(NavigationContext context)
        {
            if (this.IsNewDependant(context))
            {
                var id = context.GetDependantId();
                return this.DependantId == id;
            }

            var data = context.GetData();
            return this.Equals(data);
        }

        public override void OnNavigatedTo(NavigationContext context)
        {
            if (this.IsNewDependant(context))
            {
                this.DependantId = context.GetDependantId();
            }
            else if (this.IsExistingDependantWithoutViewInstance(context))
            {
                var data = context.GetData();
                this.MapFrom(data);
            }

            base.OnNavigatedTo(context);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if (this.isBackingUp)
            {
                navigationContext.Parameters.Add(DesktopConstants.DependantDataParameter, this);
            }
            this.isBackingUp = false;
            base.OnNavigatedFrom(navigationContext);
        }

        private bool IsNewDependant(NavigationContext context)
        {
            return context.HasDependantId();
        }

        private bool IsExistingDependantWithoutViewInstance(NavigationContext context)
        {
            var data = context.GetData();
            return !this.Equals(data);
        }

        public override int GetHashCode()
        {
            // Does not call base.GetHashCode on purpose, since navigation in nested forms happens with 'DependantId'.
            // It is not readonly, because we have to provide the 'DependantId' as NavigationParameter when adding
            // the dependant entity. This is necessary, because Prism's Journal keeps only RequestNavigation calls
            // in the back steps list, rather than some sort of view ID. Hence if we want to be able to back-up to the
            // same newly created view instance we have to have provided ID in the first place. Otherwise Prism will
            // simply create new instance, causing the infamous back-up problem.

            // Also do not snapshot this as DependantId can change (not readonly).
            return (this.GetType().ToString() + this.DependantId).GetHashCode();
        }
    }
}
