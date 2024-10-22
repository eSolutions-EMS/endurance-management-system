using Not.Domain.Ports;
using Not.Injection;
using Not.Startup;

namespace NTS.Domain.Core.Configuration;

public class StaticOptions : IStartupInitializer, ISingletonService
{
    
    static Model? _options;
    public static IRegionalConfiguration? RegionalConfiguration { get; set; }
    public static Country[] Countries => _options?.Countries ?? [];

    public static bool IsRfidDetectionEnabled()
    {
        return _options != default && _options.DetectionMode == DetectionMode.Rfid;
    }

    public static bool IsVisionDetectionEnabled()
    {
        return _options != default && _options.DetectionMode == DetectionMode.ComputerVision;
    }

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

    static bool ShouldUseRegionalConfiguration(CompetitionRuleset ruleset)
    {
        return ruleset == CompetitionRuleset.Regional && RegionalConfiguration != null;
    }

    #region Instance is used in order to be initialized on Startup

    readonly IStaticOptionsProvider<Model> _provider;

    public StaticOptions(IStaticOptionsProvider<Model> provider)
    {
        _provider = provider;
    }

    public void RunAtStartup()
    {
        _options = _provider.Get();
        RegionalConfiguration = RegionalConfigurationProvider.Get(_options.SelectedCountry);
    }

    #endregion

    public class Model
    {
        public Country[] Countries { get; set; } = [];  
        public Country? SelectedCountry { get; set; }
        public DetectionMode DetectionMode { get; set; }
    }
}
