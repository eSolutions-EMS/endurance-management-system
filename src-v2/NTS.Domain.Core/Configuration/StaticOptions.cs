using Not.Domain.Ports;
using Not.Injection;
using Not.Startup;

namespace NTS.Domain.Core.Configuration;

public class StaticOptions : IStartupInitializer, ISingletonService
{
    readonly IStaticOptionsProvider<Model> _provider;
    
    static Model? _options;
    public static IRegionalConfiguration? RegionalConfiguration { get; set; }
    public static DetectionConfiguration? DetectionConfiguration { get; set; }

    public static bool ShouldOnlyUseAverageLoopSpeed(CompetitionRuleset ruleset)
    {
        if (!ShouldUseRegionalConfiguration(ruleset))
        {
            return RegionalConfiguration!.ShouldOnlyUseAverageLoopSpeed;
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

    public static bool RfidDetection()
    {
        return DetectionConfiguration.IsRfid();
    }

    static bool ShouldUseRegionalConfiguration(CompetitionRuleset ruleset)
    {
        return ruleset == CompetitionRuleset.Regional && RegionalConfiguration != null;
    }

    public StaticOptions(IStaticOptionsProvider<Model> provider)
    {
        _provider = provider;
    }

    public void RunAtStartup()
    {
        _options = _provider.Get();
        RegionalConfiguration = RegionalConfigurationProvider.Get(_options.SelectedCountry);
        DetectionConfiguration = new DetectionConfiguration(_options.DetectionMode);
    }

    public class Model
    {
        public Country[] Countries { get; set; } = [];  
        public Country? SelectedCountry { get; set; }
        public DetectionMode DetectionMode { get; set; }
    }
}
