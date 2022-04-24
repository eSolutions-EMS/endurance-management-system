using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Controls.Manager;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Common;

public abstract class ParticipantTemplateModelBase : ViewModelBase
{
    private Visibility controlsVisibility;

    protected ParticipantTemplateModelBase(Participation participation)
    {
        this.Participant = participation.Participant;
        this.Number = Participant.Number;
        var viewModels = Performance
            .GetAll(participation)
            .Select(perf => new PerformanceControlModel(perf));
        this.Performances.AddRange(viewModels);
        var aggregate = participation.Aggregate();
        if (aggregate.IsDisqualified)
        {
            this.Color = new SolidColorBrush(Colors.Red);
            this.DisqualifyCode = aggregate.DisqualifiedCode;
        }
    }

    public ObservableCollection<PerformanceControlModel> Performances { get; } = new();
    public PerformanceControlModel Kur => Performances.FirstOrDefault();
    public Visibility ControlsVisibility
    {
        get => this.controlsVisibility;
        protected set
        {
            this.controlsVisibility = value;
            foreach (var performance in this.Performances)
            {
                performance.EditVisibility = value;
            }
        }
    }
    public SolidColorBrush Color { get; } = new(Colors.Black);
    public string DisqualifyCode { get; }

    protected Participant Participant { get; }

    private readonly int number;

    public int Number
    {
        get => this.number;
        private init => this.SetProperty(ref this.number, value);
    }
}
