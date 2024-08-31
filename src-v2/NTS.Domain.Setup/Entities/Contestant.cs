using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;
public class Contestant : DomainEntity, ISummarizable
{
    public static Contestant Create(DateTimeOffset? newStart, bool isUnranked, Combination combination) => new(newStart, isUnranked, combination);
    public static Contestant Update(int id, DateTimeOffset? newStart, bool isUnranked, Combination combination) => new(id, newStart, isUnranked, combination);
    private List<Combination> _combinations = new();

    [JsonConstructor]
    private Contestant(int id, DateTimeOffset? startTimeOverride, bool isUnranked, Combination combination) : this(startTimeOverride, isUnranked, combination) 
    {
        Id = id;
    }
    private Contestant(DateTimeOffset? startTimeOverride, bool isUnranked, Combination combination)
    {
        if ( startTimeOverride != null && startTimeOverride.Value.DateTime.CompareTo(DateTime.Today) < 0)
        {
            throw new DomainException(nameof(StartTimeOverride), "Start time cannot be in the past");
        }
        StartTimeOverride = startTimeOverride;
        IsUnranked = isUnranked;
        Combination = combination;
    }

    public Combination Combination {  get; private set; }
    public DateTimeOffset? StartTimeOverride { get; private set; }
    public bool IsUnranked { get; private set; }

    public IReadOnlyList<Combination> Combinations
    {
        get => _combinations.AsReadOnly();
        private set => _combinations = value.ToList();
    }

    public string Summarize()
    {
        var summary = new Summarizer(this);
        return summary.ToString();
    }

    public override string ToString()
    {
        var startTimeMessage = StartTimeOverride != null 
            ? $"start: {StartTimeOverride.Value.ToLocalTime().TimeOfDay} "
            : null;
        var isUnrankedMessage = IsUnranked
            ? "not-ranked"
            : null;
        return Combine(Combination, startTimeMessage, isUnrankedMessage);
    }

    public void Add(Combination child)
    {
        _combinations.Add(child);
    }

    public void Remove(Combination child)
    {
        _combinations.Remove(child);
    }

    public void Update(Combination child)
    {
        _combinations.Remove(child);

        Add(child);
    }
}
