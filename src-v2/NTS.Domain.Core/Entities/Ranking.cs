using Newtonsoft.Json;
using NTS.Domain.Core.Aggregates.Participations;
using System.Collections.ObjectModel;

namespace NTS.Domain.Core.Entities;

public class Ranking : DomainEntity, IAggregateRoot
{
    [JsonConstructor]
    private Ranking(int id) : base(id)
    {
    }
    public Ranking(Competition competition, AthleteCategory category, IEnumerable<RankingEntry> entries)
    {
        Name = competition.Name;
        Ruleset = competition.Ruleset;
        Category = category;
        Entries = new(entries.ToList());
    }

    public string Name { get; private set; }
    public CompetitionRuleset Ruleset { get; private set; }
    public AthleteCategory Category { get;private set; }
    public ReadOnlyCollection<RankingEntry> Entries { get; private set; }

    public override string ToString()
    {
        return $"{Name} {Category}: {Entries.Count}";
    }
}
