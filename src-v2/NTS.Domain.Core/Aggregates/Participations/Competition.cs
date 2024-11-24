using Not.Domain.Base;

namespace NTS.Domain.Core.Aggregates.Participations;

public record Competition : DomainObject
{
    public Competition(string name, CompetitionRuleset ruleset)
    {
        Name = name;
        Ruleset = ruleset;
    }

    public string Name { get; }
    public CompetitionRuleset Ruleset { get; }

    public override string ToString()
    {
        return $"{Name} ({Ruleset})";
    }
}
