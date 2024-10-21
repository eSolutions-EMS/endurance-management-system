using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;

public class Competition : DomainEntity, ISummarizable, IParent<Participation>, IParent<Phase>
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
        IEnumerable<Participation> participations)
        => new(id, name, type, start, ToTimeSpan(compulsoryThresholdMinutes), phases, participations);

    List<Phase> _phases = [];
    List<Participation> _participations = [];

    [JsonConstructor]
    private Competition(
        int id, 
        string? name, 
        CompetitionRuleset? type,
        DateTimeOffset start,
        TimeSpan? criRecovery,
        IEnumerable<Phase> phases,
        IEnumerable<Participation> participations) : base(id)
    {
        Name = Required(nameof(Name), name);
        Type = Required(nameof(Type), type);
        Start = start;
        CompulsoryThreshold = criRecovery;
        _phases = phases.ToList();
        _participations = participations.ToList();
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

    public string Name { get; }
    public CompetitionRuleset Type { get; }
	public DateTimeOffset Start { get; }
    public TimeSpan? CompulsoryThreshold { get; }
    public IReadOnlyList<Phase> Phases => _phases.AsReadOnly();
    public IReadOnlyList<Participation> Participations => _participations.AsReadOnly();

    public string Summarize()
	{
		var summary = new Summarizer(this);
		summary.Add("phases".Localize(), _phases);
		summary.Add("contestants".Localize(), _participations);
		return summary.ToString();
	}

	public override string ToString()
	{
        return Combine(
            $"{Name} ({Phases.Count})",
            Type.ToString().Localize(),
            $"{Start:g}");
	}

    public void Add(Participation child)
    {
        _participations.Add(child);
    }

    public void Remove(Participation child)
    {
        _participations.Remove(child);
    }

    public void Update(Participation child)
    {
        _participations.Remove(child);
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