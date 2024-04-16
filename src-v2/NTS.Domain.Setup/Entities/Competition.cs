using NTS.Domain.Events.Start;
using NTS.Domain.Setup.Import;
using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;

public class Competition : DomainEntity, ISummarizable, IParent<Contestant>
{

    public static Competition Create(string name, CompetitionType type, DateTimeOffset start) => new(name,type, start);
    public static Competition Update( int id, string name, CompetitionType type, DateTimeOffset start) => new(id, name, type, start);

    private List<Loop> _loops = new();
    private List<Contestant> _contestants = new();

    [JsonConstructor]
    private Competition(int id, string name, CompetitionType type, DateTimeOffset startTime)
    {
        Id = id;
        if (type == 0)
        {
            throw new DomainException(nameof(Type), "Competition type must have a value different from 0.");
        }

        if (startTime.DateTime.CompareTo(DateTime.Today) < 0)
        {
            throw new DomainException(nameof(StartTime), "Date of Competition cannot be in the past");
        }

        this.Name = name;
        this.Type = type;
        this.StartTime = startTime;
    }

    private Competition(string name, CompetitionType type, DateTimeOffset startTime)
    {
        if (type == 0)
        {
            throw new DomainException(nameof(Type), "Competition type must have a value different from 0.");
        }

        if (startTime.DateTime.CompareTo(DateTime.Today) < 0)
        {
            throw new DomainException(nameof(StartTime), "Date of Competition cannot be in the past");
        }

        this.Name = name;
        this.Type = type;
        this.StartTime = startTime;
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
}

public enum CompetitionType
{
	FEI = 1,
	National = 2,
    Qualification = 3
}
