using Not.Localization;

namespace NTS.Domain.Core.Entities;

public class RankingEntry : DomainEntity
{
    //TODO: works without JsonConstructor. Test if attribute is even necessary in new setup
    private RankingEntry(int id, Participation participation, bool isNotRanked) : base(id)
    {
        Participation = participation;
        IsNotRanked = isNotRanked;
    }
    public RankingEntry(Participation participation, bool isNotRanked) : this(GenerateId(), participation, isNotRanked)
    {
    }

    public Participation Participation { get; private set; }
    public bool IsNotRanked { get; private set; } 

    public override string ToString()
    {
        var message = IsNotRanked
            ? $"({"not ranked".Localize()}) "
            : "";
        return message + Participation.ToString();
    }
}
