using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Gateways.Desktop.Print.Performances;
using EnduranceJudge.Gateways.Desktop.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Controls.Manager;

public class ParticipationGridModel
{
    private readonly IExecutor executor;
    public ParticipationGridModel(Participation participation, int? columns = null)
    {
        this.executor = StaticProvider.GetService<IExecutor>();
        this.Participant = participation.Participant;

        // TODO: remove
        this.Number = this.Participant.Number;
        var viewModels = Performance
            .GetAll(participation)
            .Select(perf => new PerformanceColumnModel(perf))
            .ToList();
        this.Performances.AddRange(viewModels);
        if (columns.HasValue)
        {
            this.EmptyColumns = columns.Value - viewModels.Count;
        }
        if (this.EmptyColumns < 0)
        {
            throw new Exception($"Participant {this.Number} as more performances than columns.");
        }

        var aggregate = participation.Aggregate();
        if (aggregate.IsDisqualified)
        {
            this.Color = new SolidColorBrush(Colors.Red);
            this.DisqualifyCode = aggregate.DisqualifiedCode;
        }
    }
    public int EmptyColumns { get; }
    public string Number { get; }
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
