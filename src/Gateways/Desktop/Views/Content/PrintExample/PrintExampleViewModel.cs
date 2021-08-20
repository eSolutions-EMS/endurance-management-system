using EnduranceJudge.Gateways.Desktop.Core;
using Prism.Commands;
using System.Windows.Controls;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.PrintExample
{
    public class PrintExampleViewModel : ViewModelBase
    {
        public PrintExampleViewModel()
        {
            this.Print = new DelegateCommand<Visual>(this.PrintAction);
        }

        public DelegateCommand<Visual> Print { get; }

        private void PrintAction(Visual control)
        {
            var dialog = new PrintDialog();
            dialog.PrintVisual(control, "print example");
        }
    }
}
