using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Gateways.Desktop.Print.Performances;
using EnduranceJudge.Gateways.Desktop.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Controls.Manager;

public class ParticipationGridModel
{
    private readonly IExecutor executor;
    public ParticipationGridModel(Participation participation)
    {
        this.executor = StaticProvider.GetService<IExecutor>();
        this.Participant = participation.Participant;

        // TODO: remove
        this.Number = this.Participant.Number;
        var viewModels = Performance
            .GetAll(participation)
            .Select(perf => new PerformanceColumnModel(perf));
        this.Performances.AddRange(viewModels);

        var aggregate = participation.Aggregate();
        if (aggregate.IsDisqualified)
        {
            this.Color = new SolidColorBrush(Colors.Red);
            this.DisqualifyCode = aggregate.DisqualifiedCode;
        }
    }
    public int Number { get; }
    public string DisqualifyCode { get; }
    public Participant Participant { get; }
    public SolidColorBrush Color { get; } = new(Colors.Black);
    public ObservableCollection<PerformanceColumnModel> Performances { get; } = new();

    public void PrintAction()
    {
        this.executor.Execute(() =>
        {
            var printer = new ParticipationPrinter(this);
            printer.PreviewDocument();
        });
    }
}
