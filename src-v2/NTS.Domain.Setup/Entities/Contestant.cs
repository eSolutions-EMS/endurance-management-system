using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;
public class Contestant : DomainEntity, ISummarizable, IParent<Tandem>
{
    public static Contestant Create(DateTimeOffset newStart) => new(newStart);
    public static Contestant Update(int id, DateTimeOffset newStart) => new(id, newStart);

    private List<Tandem> _tandems = new();

    [JsonConstructor]
    private Contestant(int id, DateTimeOffset startTimeOverride)
    {

        if (startTimeOverride.DateTime.CompareTo(DateTime.Today) < 0)
        {
            throw new DomainException(nameof(StartTimeOverride), "Start time cannot be in the past");
        }
        Id = id;
        StartTimeOverride = startTimeOverride;
    }
    private Contestant(DateTimeOffset startTimeOverride)
    {

        if (startTimeOverride.DateTime.CompareTo(DateTime.Today) < 0)
        {
            throw new DomainException(nameof(StartTimeOverride), "Start time cannot be in the past");
        }

        StartTimeOverride = startTimeOverride;
    }
    public Tandem ContestantHorsePair {  get; private set; }

    public DateTimeOffset StartTimeOverride { get; private set; }

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
        //sb.Append($"{LocalizationHelper.Get(this.Type)}, {"Loops".Localize()}: {this.Loops.Count}, {"Starters".Localize()}: {this.Contestants.Count}, ");
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
