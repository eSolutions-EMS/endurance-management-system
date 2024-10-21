using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;

public class Participation : DomainEntity, ISummarizable
{
    public static Participation Create(DateTimeOffset? newStart, bool isUnranked, Combination? combination)
        => new(newStart, isUnranked, combination);

    public static Participation Update(int id, DateTimeOffset? newStart, bool isUnranked, Combination? combination)
        => new(id, newStart, isUnranked, combination);
    
    [JsonConstructor]
    private Participation(int id, DateTimeOffset? startTimeOverride, bool isUnranked, Combination? combination) : base(id)
    {
        Id = id;
        StartTimeOverride = startTimeOverride;
        IsNotRanked = isUnranked;
        Combination = Required(nameof(Combination), combination);
    }

    private Participation(DateTimeOffset? startTimeOverride, bool isUnranked, Combination? combination) : this(
        GenerateId(),
        Validate(startTimeOverride),
        isUnranked,
        combination)
    {
        throw new DomainException(nameof(IsNotRanked), "test"); // TODO: remove this test shit
    }

    static DateTimeOffset? Validate(DateTimeOffset? startTimeOverride)
    {
        if (startTimeOverride != null && startTimeOverride.Value.DateTime.CompareTo(DateTime.Today) < 0)
        {
            throw new DomainException(nameof(StartTimeOverride), "Start time cannot be in the past");
        }
        return startTimeOverride;
    }

    public Combination Combination { get; }
    public DateTimeOffset? StartTimeOverride { get; }
    public bool IsNotRanked { get; }

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
        var isUnrankedMessage = IsNotRanked
            ? "not-ranked"
            : null;
        return Combine(Combination, startTimeMessage, isUnrankedMessage);
    }
}
