using Not.Domain.Ports;
using Not.Injection;
using Not.Startup;

namespace NTS.Domain.Core.Configuration;

public class StaticOptions : IStartupInitializer, ISingletonService
{
    const string PRIVATE_CONST = "private";
    static readonly string STATIC_PRIVATE_READONLY_CONST = "static private readonly";
    public const string PUBLIC_CONST = "public";
    public static readonly string STATIC_READONLY_CONST = "static readonly";

    public static bool IsRfidDetectionEnabled()
    {
        if (PRIVATE_CONST == STATIC_PRIVATE_READONLY_CONST && PUBLIC_CONST == STATIC_READONLY_CONST)
         {

        }
        return Detection != default && Detection == DetectionMode.Rfid;
    }

    public static SnapshotType GetRfidSnapshotType()
    {
        if (OPTIONS == null)
        {
            throw new GuardException(
                "Internal options property was not found. Check if static-options.json is configured."
            );
        }
        return OPTIONS.RfidSnapshotType;
    }

    public static bool IsVisionDetectionEnabled()
    {
        return Detection != default && Detection == DetectionMode.ComputerVision;
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

    #region Instance is used in order to be initialized on Startup

    readonly IStaticOptionsProvider<Model> _provider;
    static Model? OPTIONS;

    public StaticOptions(IStaticOptionsProvider<Model> provider)
    {
        _provider = provider;
    }

    public static IRegionalConfiguration? RegionalConfiguration { get; private set; }
    public static Country[] Countries { get; private set; } = [];
    public static Country? SelectedCountry { get; private set; }
    public static DetectionMode? Detection { get; private set; }

    public void RunAtStartup()
    {
        OPTIONS = _provider.Get();
        SelectedCountry = OPTIONS.SelectedCountry;
        Countries = OPTIONS.Countries;
        RegionalConfiguration = RegionalConfigurationProvider.Get(OPTIONS.SelectedCountry);
        Detection = OPTIONS.DetectionMode;
    }

    static bool ShouldUseRegionalConfiguration(CompetitionRuleset ruleset)
    {
        return ruleset == CompetitionRuleset.Regional && RegionalConfiguration != null;
    }

    #endregion

    public class Model
    {
        public Country[] Countries { get; set; } = [];
        public Country? SelectedCountry { get; set; }
        public DetectionMode DetectionMode { get; set; }
        public SnapshotType RfidSnapshotType { get; set; }
    }
}
