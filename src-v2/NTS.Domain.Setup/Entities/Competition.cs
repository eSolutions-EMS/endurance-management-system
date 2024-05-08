using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;

public class Competition : DomainEntity, ISummarizable, IParent<Contestant>, IParent<Loop>
{
    public static Competition Create(string name, CompetitionType type, DateTimeOffset start) => new(name,type, start);
    public static Competition Update(int id, string name, CompetitionType type, DateTimeOffset start, IEnumerable<Loop> loops, IEnumerable<Contestant> contestants)
        => new(id, name, type, start, loops, contestants);

    private List<Loop> _loops = new();
    private List<Contestant> _contestants = new();

    [JsonConstructor]
    private Competition(int id, string name, CompetitionType type, DateTimeOffset startTime, IEnumerable<Loop> loops, IEnumerable<Contestant> contestants)
        : this(name, type, startTime)
    {
        Id = id;
        _loops = loops.ToList();
        _contestants = contestants.ToList();
    }
    private Competition(string name, CompetitionType type, DateTimeOffset startTime)
    {
        if (type == default)
        {
            throw new DomainException(nameof(type), "Competition Type is required");
        }

        if (startTime.DateTime < DateTime.Today)
        {
            throw new DomainException(nameof(DateTime), "Competition date cannot be in the past");
        }

        Name = name;
        Type = type;
        StartTime = startTime;
    }

    public string Name { get; private set; }
    public CompetitionType Type { get; private set; }
	public DateTimeOffset StartTime { get; private set; }
   
    public IReadOnlyList<Loop> Loops
    {
        get => _loops.AsReadOnly();
        private set => _loops = value.ToList();
    }
    public IReadOnlyList<Contestant> Contestants
    {
        get => _contestants.AsReadOnly();
        private set => _contestants = value.ToList();
    }

    public string Summarize()
	{
		var summary = new Summarizer(this);
		summary.Add("Loops".Localize(), this._loops);
		summary.Add("Contestants".Localize(), this._contestants);
		return summary.ToString();
	}
	public override string ToString()
	{
		var sb = new StringBuilder();
		sb.Append($"{LocalizationHelper.Get(this.Type)}, {"Loops".Localize()}: {this.Loops.Count}, {"Starters".Localize()}: {this.Contestants.Count}, ");
		sb.Append($"{"Start".Localize()}: {this.StartTime.ToString("f", CultureInfo.CurrentCulture)}");
		return sb.ToString();
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

    public void Add(Loop child)
    {
        _loops.Add(child);
    }

    public void Remove(Loop child)
    {
        _loops.Remove(child);
    }

    public void Update(Loop child)
    {
        _loops.Remove(child);
        Add(child);
    }
}