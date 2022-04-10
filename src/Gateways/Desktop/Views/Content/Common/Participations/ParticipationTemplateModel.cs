using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Gateways.Desktop.Print.Performances;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Common.Participations;

public class ParticipationTemplateModel : ParticipantTemplateModelBase
{
    private readonly IExecutor executor;
    public ParticipationTemplateModel(Participation participation, IExecutor executor)
        : base(participation)
    {
        this.executor = executor;
        this.Print = new DelegateCommand(this.PrintAction);
    }

    public DelegateCommand Print { get; }

    private void PrintAction()
    {
        this.executor.Execute(() =>
        {
            this.ControlsVisibility = Visibility.Collapsed;
            var printer = new PerfomancePrinter(this);
            printer.PreviewDocument();
        });
        this.ControlsVisibility = Visibility.Visible;
    }
}
