using EnduranceJudge.Core.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace EnduranceJudge.Gateways.Desktop.Core
{
    public abstract class ViewModelBase : BindableBase, INavigationAware, IObject
    {
        protected ViewModelBase()
        {
            this.ObjectId = Guid.NewGuid();
        }

        protected IRegionNavigationJournal Journal { get; private set; }

        public DelegateCommand NavigateForward => new DelegateCommand(this.NavigateForwardAction);
        public DelegateCommand NavigateBack => new DelegateCommand(this.NavigateBackAction);

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.Journal = navigationContext.NavigationService.Journal;
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
            => true;

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        protected virtual void NavigateForwardAction()
        {
            this.Journal?.GoForward();
        }

        protected virtual void NavigateBackAction()
        {
            this.Journal?.GoBack();
        }

        public Guid ObjectId { get; }

        public bool ObjectEquals(IObject other)
            => this.ObjectId == other.ObjectId;

        public virtual bool Equals(IObject other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other.GetType() != this.GetType())
            {
                return false;
            }

            return this.ObjectId.Equals(other.ObjectId);
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((IObject)other);
        }

        public override int GetHashCode()
            => (this.GetType().ToString() + this.ObjectId).GetHashCode();

    }
}
