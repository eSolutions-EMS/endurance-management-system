using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Gateways.Desktop.Core;
using System.Collections.ObjectModel;
using System.Linq;
using static EnduranceJudge.Localization.Translations.Words;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Common;

public class ParticipantTemplateModelBase<TPerformanceTemplate> : ViewModelBase
{
    public ParticipantTemplateModelBase(Participant participant)
    {
        this.Number = participant.Number;
        this.UpdatePhases(participant.Participation);
    }

    public ObservableCollection<string> PhaseLengths { get; } = new();
    public ObservableCollection<TPerformanceTemplate> Performances { get; } = new();

    private readonly int number;

    private void UpdatePhases(Participation participation)
    {
        var count = 0;
        var lengths = participation.Performances.Select(x =>
        {
            count++;
            var value = $"{GATE.ToUpper()}{count}/{x.Phase.LengthInKm}";
            return value;
        });
        var viewModels = participation.Performances.MapEnumerable<TPerformanceTemplate>();

        this.Performances.AddRange(viewModels);
        this.PhaseLengths.AddRange(lengths);
    }

    public int Number
    {
        get => this.number;
        private init => this.SetProperty(ref this.number, value);
    }
}
