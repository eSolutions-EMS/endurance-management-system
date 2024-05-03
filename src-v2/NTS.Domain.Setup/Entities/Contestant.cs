using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;
public class Contestant : DomainEntity, ISummarizable
{
    public static Contestant Create(DateTimeOffset? newStart, bool isUnranked) => new(newStart, isUnranked);
    public static Contestant Update(int id, DateTimeOffset? newStart, bool isUnranked) => new(id, newStart, isUnranked);

    private List<Tandem> _tandems = new();

    [JsonConstructor]
    private Contestant(int id, DateTimeOffset? startTimeOverride, bool isUnranked) : this(startTimeOverride, isUnranked) 
    {
        Id = id;
    }
    private Contestant(DateTimeOffset? startTimeOverride, bool isUnranked)
    {
        if ( startTimeOverride != null && startTimeOverride.Value.DateTime.CompareTo(DateTime.Today) < 0)
        {
            throw new DomainException(nameof(StartTimeOverride), "Start time cannot be in the past");
        }
        StartTimeOverride = startTimeOverride;
        IsUnranked = isUnranked;
    }
    public Tandem ContestantHorsePair {  get; private set; }

    public DateTimeOffset? StartTimeOverride { get; private set; }

    public Boolean IsUnranked { get; private set; }

    public IReadOnlyList<Tandem> Tandems
    {
        get => _tandems.AsReadOnly();
        private set => _tandems = value.ToList();
    }

    public string Summarize()
    {
        var summary = new Summarizer(this);
        return summary.ToString();
    }
    public override string ToString()
    {
        var sb = new StringBuilder();
        var startTimeMessage = StartTimeOverride == null ? "" : "StartTime: " + $"{StartTimeOverride.Value.ToLocalTime().TimeOfDay}";
        var IsUnrankedMessage = IsUnranked ? "Unranked: Yes" : "Unranked: No";
        sb.Append($"Contestant -> Tandem: *isn't configured yet* {startTimeMessage} {IsUnrankedMessage} ");
        //sb.Append($"{"Start".Localize()}: {this.StartTime.ToString("f", CultureInfo.CurrentCulture)}");
        return sb.ToString();
    }

    public void Add(Tandem child)
    {
        _tandems.Add(child);
    }

    public void Remove(Tandem child)
    {
        _tandems.Remove(child);
    }

    public void Update(Tandem child)
    {
        _tandems.Remove(child);

        Add(child);
    }
}
