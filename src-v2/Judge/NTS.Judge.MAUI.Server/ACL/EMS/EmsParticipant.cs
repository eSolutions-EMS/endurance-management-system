using System.Collections.ObjectModel;

namespace NTS.Judge.MAUI.Server.ACL.EMS;

public class EmsParticipant : EmsDomainBase<EmsParticipantException>, IEmsParticipantState
{
    public const int DEFAULT_MAX_AVERAGE_SPEED = 16;
    private const string NAME_FORMAT = "{0} - {1} with {2}";

    private ObservableCollection<EmsLapRecord> lapRecords = new();
    private readonly ReadOnlyObservableCollection<EmsLapRecord> lapRecordsReadonly;

    private EmsParticipant()
    {
        lapRecordsReadonly = new(lapRecords);
    }
    public EmsParticipant(EmsAthlete athlete, EmsHorse horse, IEmsParticipantState state = null) : base(default)
    {
        lapRecordsReadonly = new ReadOnlyObservableCollection<EmsLapRecord>(lapRecords);
        Athlete = athlete;
        Horse = horse;
        MaxAverageSpeedInKmPh = state?.MaxAverageSpeedInKmPh;
        if (!int.TryParse(state?.Number, out var _))
        {
            throw EmsHelper.Create<EmsParticipantException>(
                $"Invalid '{nameof(Number)}' - '{state?.Number}'. Please enter a valid number");
        }
        Number = state?.Number;
        Unranked = state?.Unranked ?? false;
    }

    public bool Unranked { get; internal set; }
    public string Number { get; internal set; }
    public int? MaxAverageSpeedInKmPh { get; internal set; }
    public EmsHorse Horse { get; internal set; }
    public EmsAthlete Athlete { get; internal set; }
    public ObservableCollection<EmsLapRecord> LapRecords
    {
        get => lapRecords;
        private set => lapRecords = new ObservableCollection<EmsLapRecord>(value.ToList());
    }

    public void Add(EmsLapRecord record)
        => lapRecords.Add(record);

    public string Name => FormatName(Number, Athlete.Name, Horse.Name);

    public static string FormatName(string number, string athleteName, string horseName)
    {
        return string.Format(NAME_FORMAT, number, athleteName, horseName);
    }

    public void __REMOVE_RECORDS__()
    {
        lapRecords.Clear();
    }

    // Metadata for stats
    public Dictionary<EmsWitnessEventType, List<int>> DetectedHead { get; } = new()
    {
        { EmsWitnessEventType.Arrival, new List<int>() },
        { EmsWitnessEventType.VetIn, new List<int>() },
    };
}
