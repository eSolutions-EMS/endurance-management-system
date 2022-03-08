using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Localization.Strings;
using System.Collections.Generic;
using System.Reflection;

namespace EnduranceJudge.Localization;

public static class LocalizationConstants
{
    public const string DEFAULT_COUNTRY_CODE = "BGR";
    public const string STRINGS_FILENAME = "strings.txt";

    internal static readonly Dictionary<string, string> PLACEHOLDERS_VALUES = new()
    {
        { "{entities-athlete}",                Entities.ATHLETE_ENTITY },
        { "{entities-athlete-plurals}",        Entities.ATHLETES_ENTITY },
        { "{entities-competition}",            Entities.COMPETITION_ENTITY },
        { "{entities-competition-plurals}",    Entities.COMPETITIONS_ENTITY },
        { "{entities-country}",                Entities.COUNTRY_ENTITY },
        { "{entities-country-plurals}",        Entities.COUNTRIES_ENTITY },
        { "{entities-event}",                  Entities.ENDURANCE_EVENT_ENTITY },
        { "{entities-event-plurals}",          Entities.ENDURANCE_EVENTS_ENTITY },
        { "{entities-horse}",                  Entities.HORSE_ENTITY },
        { "{entities-horse-plurals}",          Entities.HORSES_ENTITY },
        { "{entities-participant}",            Entities.PARTICIPANT_ENTITY },
        { "{entities-participant-plurals}",    Entities.PARTICIPANTS_ENTITY },
        { "{entities-participation}",          Entities.PARTICIPATION_ENTITY },
        { "{entities-participation-plurals}",  Entities.PARTICIPATIONS_ENTITY },
        { "{entities-performance}",            Entities.PERFORMANCE_ENTITY },
        { "{entities-performance-plurals}",    Entities.PERFORMANCES_ENTITY },
        { "{entities-personnel}",              Entities.PERSONNEL_ENTITY },
        { "{entities-result}",                 Entities.RESULT_ENTITY },
        { "{entities-result-plurals}",         Entities.RESULTS_ENTITY },
        { "{entities-phase}",                  Entities.PHASE_ENTITY },
        { "{entities-phase-plurals}",          Entities.PHASES_ENTITY },
        { "{words-update}",                    Words.UPDATE },
        { "{words-import}",                    Words.IMPORT },
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
