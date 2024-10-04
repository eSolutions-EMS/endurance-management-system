using Not.Domain.Ports;
using Not.Injection;
using Not.Startup;

namespace NTS.Domain.Core.Configuration;

public class StaticOptions : IStartupInitializer, ISingletonService
{
    private const string RFID = "Rfid";
    private const string Vision = "ComputerVision";

    readonly IStaticOptionsProvider<Model> _provider;
    
    static Model? _options;
    public static IRegionalConfiguration? RegionalConfiguration { get; set; }
    public static bool RfidDetectionEnabled => _options != default && _options.DetectionMode.ToString() == RFID;
    public static bool VisionDetectionEnabled => _options != default &&  _options.DetectionMode.ToString() == Vision;

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

    public StaticOptions(IStaticOptionsProvider<Model> provider)
    {
        _provider = provider;
    }

    public void RunAtStartup()
    {
        _options = _provider.Get();
        RegionalConfiguration = RegionalConfigurationProvider.Get(_options.SelectedCountry);
    }

    public class Model
    {
        public Country[] Countries { get; set; } = [];  
        public Country? SelectedCountry { get; set; }
        public DetectionMode DetectionMode { get; set; }
    }
}
