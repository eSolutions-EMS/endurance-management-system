using EnduranceJudge.Core.ConventionalServices;
using System.Windows.Controls;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Services
{
    public class Printer : IPrinter
    {
        public void Print(Visual visual)
        {
            var dialog = new PrintDialog();
            dialog.PrintVisual(visual, "endurance-judge");
        }
    }

    public interface IPrinter : IService
    {
        void Print(Visual visual);
    }
}
