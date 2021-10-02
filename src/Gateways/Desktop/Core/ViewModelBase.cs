using EnduranceJudge.Core.Models;
using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Gateways.Desktop.Core.Objects;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace EnduranceJudge.Gateways.Desktop.Core
{
    public abstract class ViewModelBase : BindableBase, INavigationAware, IObject
    {
        private readonly int objectUniqueCode;

        protected ViewModelBase()
        {
            this.EventAggregator = ServiceProvider.GetService<IEventAggregator>();
            this.ObjectId = Guid.NewGuid();
            this.objectUniqueCode = ObjectUtilities.GetUniqueObjectCode(this);
        }

        protected IEventAggregator EventAggregator { get; }
        protected IRegionNavigationJournal Journal { get; private set; }

        public Guid ObjectId { get; }
        public DelegateCommand NavigateForward => new DelegateCommand(this.NavigateForwardAction);
        public DelegateCommand NavigateBack => new DelegateCommand(this.NavigateBackAction);

        public virtual void OnNavigatedTo(NavigationContext context)
        {
            this.Journal = context.NavigationService.Journal;
        }
        public virtual bool IsNavigationTarget(NavigationContext context)
        {
            return false;
        }
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

        protected virtual void ValidationError(string message)
        {
            this.EventAggregator
                .GetEvent<ErrorEvent>()
                .Publish(message);
        }

        public override bool Equals(object other)
        {
            return this.Equals(other as IObject);
        }
        public bool Equals(IObject other)
        {
            return ObjectUtilities.IsEqual(this, other);
        }
        public override int GetHashCode()
        {
            return this.objectUniqueCode;
        }
    }
}
