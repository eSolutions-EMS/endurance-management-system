using NTS.Domain.Core.Aggregates.Participations;
using System.Collections.ObjectModel;

namespace NTS.Domain.Core.Entities;

public class Classification : DomainEntity, IAggregateRoot
{
    public Classification(string name, AthleteCategory category, IEnumerable<ClassificationEntry> entries)
    {
        Name = name;
        Category = category;
        Entries = new(entries.ToList());
    }

    public string Name { get; private set; }
    public AthleteCategory Category { get;private set; }
    public ReadOnlyCollection<ClassificationEntry> Entries { get; }

    public override string ToString()
    {
        return $"{Name} {Category}: {Entries.Count}";
    }
}
