using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using Prism.Regions;

namespace EnduranceJudge.Gateways.Desktop.Views.Error
{
    public class ErrorViewModel : ViewModelBase
    {
        private string message;

        public override void OnNavigatedTo(NavigationContext context)
        {
            base.OnNavigatedTo(context);
            this.Message = context.GetMessage();
        }

        public string Message
        {
            get => this.message;
            set => this.SetProperty(ref this.message, value);
        }
    }
}
