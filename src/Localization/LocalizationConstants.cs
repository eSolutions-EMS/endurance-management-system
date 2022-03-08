using EnduranceJudge.Localization.Translations;
using System.Collections.Generic;

namespace EnduranceJudge.Localization;

public static class LocalizationConstants
{
    public const string DEFAULT_COUNTRY_CODE = "BGR";
    public const string TRANSLATION_FILE_NAME = "translation-values.txt";

    internal static readonly Dictionary<string, string> PLACEHOLDERS_VALUES = new()
    {
        { "{athlete-placeholder}",                Entities.ATHLETE },
        { "{athlete-plurals-placeholder}",        Entities.ATHLETES },
        { "{competition-placeholder}",            Entities.COMPETITION },
        { "{competition-plurals-placeholder}",    Entities.COMPETITIONS },
        { "{country-placeholder}",                Entities.COUNTRY },
        { "{country-plurals-placeholder}",        Entities.COUNTRIES },
        { "{event-placeholder}",                  Entities.ENDURANCE_EVENT },
        { "{event-plurals-placeholder}",          Entities.ENDURANCE_EVENTS },
        { "{horse-placeholder}",                  Entities.HORSE },
        { "{horse-plurals-placeholder}",          Entities.HORSES },
        { "{participant-placeholder}",            Entities.PARTICIPANT },
        { "{participant-plurals-placeholder}",    Entities.PARTICIPANTS },
        { "{participation-placeholder}",          Entities.PARTICIPATION },
        { "{participation-plurals-placeholder}",  Entities.PARTICIPATIONS },
        { "{performance-placeholder}",            Entities.PERFORMANCE },
        { "{performance-plurals-placeholder}",    Entities.PERFORMANCES },
        { "{personnel-placeholder}",              Entities.PERSONNEL },
        { "{result-placeholder}",                 Entities.RESULT },
        { "{result-plurals-placeholder}",         Entities.RESULTS },
        { "{phase-placeholder}",                  Entities.PHASE },
        { "{phase-plurals-placeholder}",          Entities.PHASES },
    };
}
