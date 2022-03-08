using EnduranceJudge.Core.Utilities;
using static EnduranceJudge.Localization.Strings;
using System.Collections.Generic;
using System.Reflection;

namespace EnduranceJudge.Localization;

public static class LocalizationConstants
{
    public const string DEFAULT_COUNTRY_CODE = "BGR";
    public const string STRINGS_FILENAME = "strings.txt";

    internal static readonly Dictionary<string, string> PLACEHOLDERS_VALUES = new()
    {
        { "{entities-athlete}",                ATHLETE_ENTITY },
        { "{entities-athlete-plurals}",        ATHLETES_ENTITY },
        { "{entities-competition}",            COMPETITION_ENTITY },
        { "{entities-competition-plurals}",    COMPETITIONS_ENTITY },
        { "{entities-country}",                COUNTRY_ENTITY },
        { "{entities-country-plurals}",        COUNTRIES_ENTITY },
        { "{entities-event}",                  ENDURANCE_EVENT_ENTITY },
        { "{entities-event-plurals}",          ENDURANCE_EVENTS_ENTITY },
        { "{entities-horse}",                  HORSE_ENTITY },
        { "{entities-horse-plurals}",          HORSES_ENTITY },
        { "{entities-participant}",            PARTICIPANT_ENTITY },
        { "{entities-participant-plurals}",    PARTICIPANTS_ENTITY },
        { "{entities-participation}",          PARTICIPATION_ENTITY },
        { "{entities-participation-plurals}",  PARTICIPATIONS_ENTITY },
        { "{entities-performance}",            PERFORMANCE_ENTITY },
        { "{entities-performance-plurals}",    PERFORMANCES_ENTITY },
        { "{entities-personnel}",              PERSONNEL_ENTITY },
        { "{entities-result}",                 RESULT_ENTITY },
        { "{entities-result-plurals}",         RESULTS_ENTITY },
        { "{entities-phase}",                  PHASE_ENTITY },
        { "{entities-phase-plurals}",          PHASES_ENTITY },
        { "{words-update}",                    UPDATE },
        { "{words-import}",                    IMPORT },
    };

    public static Assembly[] Assemblies
    {
        get
        {
            var assemblies = ReflectionUtilities.GetAssemblies("EnduranceJudge.Localization");
            return assemblies;
        }
    }
}
