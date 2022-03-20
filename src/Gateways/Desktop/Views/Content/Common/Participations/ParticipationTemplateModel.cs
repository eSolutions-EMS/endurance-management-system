using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Gateways.Desktop.Print.Performances;
using Prism.Commands;
using System.Collections.Generic;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Common.Participations;

public class ParticipationTemplateModel : ParticipantTemplateModelBase
{
    public ParticipationTemplateModel(IEnumerable<Performance> performances)
        : base(performances)
    {
        this.Print = new DelegateCommand(this.PrintAction);
    }

    public DelegateCommand Print { get; }

    private void PrintAction()
    {
        this.ControlsVisibility = Visibility.Collapsed;
        var printer = new PerfomancePrinter(this);
        printer.PreviewDocument();
        this.ControlsVisibility = Visibility.Visible;
    }
}
