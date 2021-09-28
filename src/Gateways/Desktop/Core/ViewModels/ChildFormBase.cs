using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Security.Cryptography;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public abstract class ChildFormBase<TView> : ParentFormBase<TView>, IChildForm
        where TView : IView
    {
        private bool isBackingUp;
        private bool isRemoving;

        protected ChildFormBase()
        {
            this.ChildId = Guid.NewGuid();
            this.Remove = new DelegateCommand(this.RemoveAction);
        }

        public DelegateCommand Remove { get; }

        public Guid? ChildId { get; private set; }

        protected override void NavigateBackAction()
        {
            this.isBackingUp = true;
            base.NavigateBackAction();
        }

        public override bool IsNavigationTarget(NavigationContext context)
        {
            if (this.IsNewChild(context))
            {
                var id = context.GetChildId();
                return this.ChildId == id;
            }

            var data = context.GetData();
            return this.Equals(data);
        }

        public override void OnNavigatedTo(NavigationContext context)
        {
            if (this.IsNewChild(context))
            {
                this.ChildId = context.GetChildId();
            }
            else if (this.IsExistingChildWithoutViewInstance(context))
            {
                var data = context.GetData();
                this.MapFrom(data);
            }

            base.OnNavigatedTo(context);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if (this.isRemoving)
            {
                navigationContext.Parameters.Add(DesktopConstants.REMOVE_PARAMETER, string.Empty);
                this.isRemoving = false;
            }
            if (this.isBackingUp)
            {
                navigationContext.Parameters.Add(DesktopConstants.CHILD_DATA_PARAMETER, this);
                this.isBackingUp = false;
            }
            base.OnNavigatedFrom(navigationContext);
        }

        private bool IsNewChild(NavigationContext context)
        {
            return context.HasChildId();
        }

        private bool IsExistingChildWithoutViewInstance(NavigationContext context)
        {
            var data = context.GetData();
            return !this.Equals(data);
        }

        private void RemoveAction()
        {
            this.isRemoving = true;
            this.NavigateBackAction();
        }

        public override int GetHashCode()
        {
            // Does not call base.GetHashCode on purpose, since navigation in nested forms happens with 'ChildId'.
            // It is not readonly, because we have to provide the 'ChildId' as NavigationParameter when adding
            // the child entity. This is necessary, because Prism's Journal keeps only RequestNavigation calls
            // in the back steps list, rather than some sort of view ID. Hence if we want to be able to back-up to the
            // same newly created view instance we have to have provided ID in the first place. Otherwise Prism will
            // simply create new instance, causing the infamous back-up problem.

            // Also do not snapshot this as ChildId can change (not readonly).
            return (this.GetType().ToString() + this.ChildId).GetHashCode();
        }
    }
}
