using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Views.Content.Common.Performances;
using System.Collections.ObjectModel;
using System.Linq;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Common;

public abstract class ParticipantTemplateModelBase : ViewModelBase
{
    protected ParticipantTemplateModelBase(Participant participant, bool allowEdit = false)
    {
        this.Number = participant.Number;
        var count = 0;
        var viewModels = participant.Participation.Performances.Select(x =>
        {
            count++;
            return new PerformanceTemplateModel(x, count, allowEdit);
        });

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
