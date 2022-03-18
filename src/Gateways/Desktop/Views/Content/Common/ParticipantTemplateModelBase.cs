using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Views.Content.Common.Performances;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Common;

public abstract class ParticipantTemplateModelBase : ViewModelBase
{
    protected ParticipantTemplateModelBase(IEnumerable<Performance> performances, bool allowEdit = false)
    {
        var list = performances.ToList();
        this.Participant = list.First().Participant;
        this.Number = Participant.Number;
        var viewModels = list.Select(perf => new PerformanceTemplateModel(perf, allowEdit));
        this.Performances.AddRange(viewModels);
    }

    public ObservableCollection<PerformanceTemplateModel> Performances { get; } = new();

    protected void ToggleEditPerformanceVisibility()
    {
        foreach (var performance in this.Performances)
        {
            performance.ToggleEditVisibility();
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
