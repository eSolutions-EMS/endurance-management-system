using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;

public class Competition : DomainEntity, ISummarizable, IParent<Contestant>, IParent<Phase>
{
    public static Competition Create(string? name, CompetitionRuleset? type, DateTimeOffset start, int? compulsoryThresholdMinutes)
        => new(name, type, start, compulsoryThresholdMinutes);

    public static Competition Update(
        int id,
        string? name,
        CompetitionRuleset? type,
        DateTimeOffset start,
        int? compulsoryThresholdMinutes,
        IEnumerable<Phase> phases,
        IEnumerable<Contestant> contestants)
        => new(id, name, type, start, ToTimeSpan(compulsoryThresholdMinutes), phases, contestants);

    List<Phase> _phases = [];
    List<Contestant> _contestants = [];

    [JsonConstructor]
    private Competition(
        int id, 
        string? name, 
        CompetitionRuleset? type,
        DateTimeOffset start,
        TimeSpan? criRecovery,
        IEnumerable<Phase> phases,
        IEnumerable<Contestant> contestants) : base(id)
    {
        Name = Required(nameof(Name), name);
        Type = Required(nameof(Type), type);
        Start = start;
        CompulsoryThreshold = criRecovery;
        _phases = phases.ToList();
        _contestants = contestants.ToList();
    }

    private Competition(string? name, CompetitionRuleset? type, DateTimeOffset start, int? compulsoryThresholdMinutes) : this(
        GenerateId(),
        name,
        type,
        IsFuture(nameof(Start), start),
        ToTimeSpan(compulsoryThresholdMinutes),
        [],
        [])
    {
    }

    static DateTimeOffset IsFuture(string field, DateTimeOffset start)
    {
        if (start <= DateTimeOffset.Now)
        {
            throw new DomainException(field, "Competition start cannot be in the past");
        }
        return start;
    }

    static TimeSpan? ToTimeSpan(int? minutes)
    {
        return minutes != null ? TimeSpan.FromSeconds(minutes.Value) : null;
    }

    public string Name { get; private set; }
    public CompetitionRuleset Type { get; private set; }
	public DateTimeOffset Start { get; private set; }
    public TimeSpan? CompulsoryThreshold { get; private set; }
    public IReadOnlyList<Phase> Phases
    {
        get => _phases.AsReadOnly();
        private set => _phases = value.ToList();
    }
    public IReadOnlyList<Contestant> Contestants
    {
        get => _contestants.AsReadOnly();
        private set => _contestants = value.ToList();
    }

    public string Summarize()
	{
		var summary = new Summarizer(this);
		summary.Add("phases".Localize(), _phases);
		summary.Add("contestants".Localize(), _contestants);
		return summary.ToString();
	}

	public override string ToString()
	{
        return Combine(
            LocalizationHelper.Get(Type),
            $"{"phases".Localize()}: {Phases.Count}",
            $"{"start".Localize()}: {Start:f}");
	}

    public void Add(Contestant child)
    {
        _contestants.Add(child);
    }

    public void Remove(Contestant child)
    {
        _contestants.Remove(child);
    }

    public void Update(Contestant child)
    {
        _contestants.Remove(child);
        Add(child);
    }

    public void Add(Phase child)
    {
        _phases.Add(child);
    }

    public void Remove(Phase child)
    {
        _phases.Remove(child);
    }

    public void Update(Phase child)
    {
        _phases.Remove(child);
        Add(child);
    }
}