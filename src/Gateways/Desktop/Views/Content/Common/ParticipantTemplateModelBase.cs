using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Views.Content.Common.Performances;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Common;

public abstract class ParticipantTemplateModelBase : ViewModelBase
{
    protected ParticipantTemplateModelBase(int number, IEnumerable<Performance> performances, bool allowEdit = false)
    {
        this.Number = number;
        var viewModels = performances.Select(perf => new PerformanceTemplateModel(perf, allowEdit));
        this.Performances.AddRange(viewModels);
    }

    public ObservableCollection<PerformanceTemplateModel> Performances { get; } = new();

    private readonly int number;

    public int Number
    {
        get => this.number;
        private init => this.SetProperty(ref this.number, value);
    }
}
