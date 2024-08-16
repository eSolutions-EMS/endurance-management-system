using NTS.Domain.Core.Aggregates.Participations;
using System.Collections.ObjectModel;

namespace NTS.Domain.Core.Entities;

public class Ranking : DomainEntity, IAggregateRoot
{
    private Ranking(int id) : base(id)
    {
    }
    public Ranking(string name, AthleteCategory category, IEnumerable<RankingEntry> entries)
    {
        Name = name;
        Category = category;
        Entries = new(entries.ToList());
    }

    public string Name { get; private set; }
    public AthleteCategory Category { get;private set; }
    public ReadOnlyCollection<RankingEntry> Entries { get; private set; }

    public override string ToString()
    {
        return $"{Name} {Category}: {Entries.Count}";
    }
}
