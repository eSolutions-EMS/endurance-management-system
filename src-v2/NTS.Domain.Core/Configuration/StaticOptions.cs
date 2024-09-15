namespace NTS.Domain.Configuration;

public static class StaticOptions
{
    public static IRegionalConfiguration? Configuration { get; set; }

    public static bool ShouldOnlyUseAverageLoopSpeed(CompetitionRuleset ruleset)
    {
        if (!ShouldUseRegionalConfiguration(ruleset))
        {
            return Configuration!.ShouldOnlyUseAverageLoopSpeed;
        }
        return false;
    }

    public static bool ShouldUseRegionalRanker(CompetitionRuleset ruleset)
    {
        if (!ShouldUseRegionalConfiguration(ruleset))
        {
            return false;
        }
        return true;
    }

    static bool ShouldUseRegionalConfiguration(CompetitionRuleset ruleset)
    {
        return ruleset == CompetitionRuleset.Regional && Configuration != null;
    }
}
