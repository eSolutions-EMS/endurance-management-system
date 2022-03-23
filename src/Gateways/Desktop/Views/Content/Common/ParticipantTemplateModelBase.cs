using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Views.Content.Common.Performances;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Common;

public abstract class ParticipantTemplateModelBase : ViewModelBase
{
    private Visibility controlsVisibility;

    protected ParticipantTemplateModelBase(IEnumerable<Performance> performances)
    {
        var list = performances.ToList();
        if (list.Count == 0)
        {
            return;
        }
        this.Participant = list.First().Participant;
        this.Number = Participant.Number;
        var viewModels = list.Select(perf => new PerformanceTemplateModel(perf));
        this.Performances.AddRange(viewModels);
    }

    public ObservableCollection<PerformanceTemplateModel> Performances { get; } = new();
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

    protected Participant Participant { get; }

    private readonly int number;

    public int Number
    {
        get => this.number;
        private init => this.SetProperty(ref this.number, value);
    }
}
