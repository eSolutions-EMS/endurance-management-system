using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Gateways.Desktop.Print.Performances;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Common;

namespace EnduranceJudge.Gateways.Desktop.Controls.Manager;

public class ParticipationGridModel : ParticipantTemplateModelBase
{
    private readonly IExecutor executor;
    public ParticipationGridModel(Participation participation, IExecutor executor)
        : base(participation)
    {
        this.executor = executor;
    }

    public void PrintAction()
    {
        this.executor.Execute(() =>
        {
            var printer = new ParticipationPrinter(this);
            printer.PreviewDocument();
        });
    }
}
