using EnduranceJudge.Localization.Translations;
using System.Collections.Generic;

namespace EnduranceJudge.Localization;

public static class LocalizationConstants
{
    public const string DEFAULT_COUNTRY_CODE = "BGR";
    public const string TRANSLATION_FILE_NAME = "translation-values.txt";

    internal static readonly Dictionary<string, string> PLACEHOLDERS_VALUES = new()
    {
        { "{entities-athlete}",                Entities.ATHLETE },
        { "{entities-athlete-plurals}",        Entities.ATHLETES },
        { "{entities-competition}",            Entities.COMPETITION },
        { "{entities-competition-plurals}",    Entities.COMPETITIONS },
        { "{entities-country}",                Entities.COUNTRY },
        { "{entities-country-plurals}",        Entities.COUNTRIES },
        { "{entities-event}",                  Entities.ENDURANCE_EVENT },
        { "{entities-event-plurals}",          Entities.ENDURANCE_EVENTS },
        { "{entities-horse}",                  Entities.HORSE },
        { "{entities-horse-plurals}",          Entities.HORSES },
        { "{entities-participant}",            Entities.PARTICIPANT },
        { "{entities-participant-plurals}",    Entities.PARTICIPANTS },
        { "{entities-participation}",          Entities.PARTICIPATION },
        { "{entities-participation-plurals}",  Entities.PARTICIPATIONS },
        { "{entities-performance}",            Entities.PERFORMANCE },
        { "{entities-performance-plurals}",    Entities.PERFORMANCES },
        { "{entities-personnel}",              Entities.PERSONNEL },
        { "{entities-result}",                 Entities.RESULT },
        { "{entities-result-plurals}",         Entities.RESULTS },
        { "{entities-phase}",                  Entities.PHASE },
        { "{entities-phase-plurals}",          Entities.PHASES },
        { "{words-update}",                    Words.UPDATE },
        { "{words-import}",                    Words.IMPORT },
    };
}
