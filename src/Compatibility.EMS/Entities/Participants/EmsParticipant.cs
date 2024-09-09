using Newtonsoft.Json;
using NTS.Compatibility.EMS.Abstractions;
using NTS.Compatibility.EMS.Entities.Athletes;
using NTS.Compatibility.EMS.Entities.Horses;
using NTS.Compatibility.EMS.Entities.LapRecords;
using System.Collections.ObjectModel;

namespace NTS.Compatibility.EMS.Entities.Participants;

public class EmsParticipant : EmsDomainBase<EmsParticipantException>, IEmsParticipantState
{
    public const int DEFAULT_MAX_AVERAGE_SPEED = 16;
    private const string NAME_FORMAT = "{0} - {1} with {2}";

    private ObservableCollection<EmsLapRecord> lapRecords = new();
    private readonly ReadOnlyObservableCollection<EmsLapRecord> lapRecordsReadonly;

    [JsonConstructor]
    private EmsParticipant()
    {
        this.lapRecordsReadonly = new(this.lapRecords);
    }
    public EmsParticipant(EmsAthlete athlete, EmsHorse horse, IEmsParticipantState state = null) : base(GENERATE_ID)
    {
        this.lapRecordsReadonly = new ReadOnlyObservableCollection<EmsLapRecord>(this.lapRecords);
        this.Athlete = athlete;
        this.Horse = horse;
        this.MaxAverageSpeedInKmPh = state?.MaxAverageSpeedInKmPh;
        if (!int.TryParse(state?.Number, out var _))
        {
            throw EmsHelper.Create<EmsParticipantException>(
                $"Invalid '{nameof(Number)}' - '{state?.Number}'. Please enter a valid number");
        }
        this.Number = state?.Number;
        this.Unranked = state?.Unranked ?? false;
    }

    public List<EmsRfidTag> RfidTags { get; internal set; } = new();
    public bool Unranked { get; internal set; }
    public string Number { get; internal set; }
    public int? MaxAverageSpeedInKmPh { get; internal set; }
    public EmsHorse Horse { get; internal set; }
    public EmsAthlete Athlete { get; internal set; }
    public ObservableCollection<EmsLapRecord> LapRecords
    {
        get => this.lapRecords;
        private set => this.lapRecords = new ObservableCollection<EmsLapRecord>(value.ToList());
    }

    public void Add(EmsLapRecord record)
        => this.lapRecords.Add(record);

    public string Name => FormatName(this.Number, this.Athlete.Name, this.Horse.Name);

    public static string FormatName(string number, string athleteName, string horseName)
    {
        return string.Format(NAME_FORMAT, number, athleteName, horseName);
    }

    public void __REMOVE_RECORDS__()
    {
        this.lapRecords.Clear();
    }

    // Metadata for stats
    public Dictionary<WitnessEventType, List<int>> DetectedHead { get; } = new()
    {
        { WitnessEventType.Arrival, new List<int>() },
        { WitnessEventType.VetIn, new List<int>() },
    };

    public override string ToString()
    {
        return Number;
    }
}
