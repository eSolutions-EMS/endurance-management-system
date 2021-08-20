using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Events;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.EnduranceEvents;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.EnduranceEvents.Listing;
using EnduranceJudge.Gateways.Desktop.Views.Content.PrintExample;
using Prism.Commands;
using Prism.Events;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Navigation
{
    public class NavigationViewModel : ViewModelBase
    {
        private readonly IEventAggregator eventAggregator;

        public NavigationViewModel(INavigationService navigation, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.NavigateToImport = new DelegateCommand(navigation.NavigateToImport);
            this.NavigateToEvent = new DelegateCommand(navigation.NavigateToEvent);

            this.NavigateToPrintExample = new DelegateCommand(navigation.ChangeTo<PrintExampleView>);
            this.NavigateToUpdateEvent = new DelegateCommand(() => navigation.ChangeTo<EnduranceEventView>(1));
            this.NavigateToPrintExample = new DelegateCommand(navigation.ChangeTo<PrintExampleView>);
            this.NavigateToEventList = new DelegateCommand(navigation.ChangeTo<EnduranceEventListView>);
            this.CloseNotification = new DelegateCommand(this.CloseNotificationAction);

            this.Subscribe();
        }

        public DelegateCommand NavigateToImport { get; }
        public DelegateCommand NavigateToEvent { get; }
        public DelegateCommand NavigateToPrintExample { get; }
        public DelegateCommand CloseNotification { get; }
        public DelegateCommand NavigateToUpdateEvent { get; }
        public DelegateCommand NavigateToEventList { get; }

        private Visibility notificationVisibility = Visibility.Hidden;
        public Visibility NotificationVisibility
        {
            get => this.notificationVisibility;
            private set => this.SetProperty(ref this.notificationVisibility, value);
        }

        private string notificationMessage;
        public string NotificationMessage
        {
            get => this.notificationMessage;
            private set => this.SetProperty(ref this.notificationMessage, value);
        }

        private void CloseNotificationAction()
        {
            this.NotificationMessage = null;
            this.NotificationVisibility = Visibility.Hidden;
        }

        private void Subscribe()
        {
            this.eventAggregator
                .GetEvent<ValidationErrorEvent>()
                .Subscribe(this.SubscribeToValidationErrors);
        }

        private void SubscribeToValidationErrors(string message)
        {
            this.NotificationMessage = message;
            this.NotificationVisibility = Visibility.Visible;
        }
    }
}
