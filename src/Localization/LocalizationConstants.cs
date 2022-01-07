using EnduranceJudge.Localization.Translations;
using System.Collections.Generic;

namespace EnduranceJudge.Localization;

public static class LocalizationConstants
{
    public const string DEFAULT_COUNTRY_CODE = "BUL";
    public const string TRANSLATION_FILE_NAME = "translation-values.txt";

    internal static Dictionary<string, string> Placeholders = new()
    {
        { nameof(Entities.ATHLETE), "<<athlete-placeholder>>" },
        { nameof(Entities.ATHLETES), "<<athlete-plurals-placeholder>>" },
        { nameof(Entities.COMPETITION), "<<competition-placeholder>>" },
        { nameof(Entities.COMPETITIONS), "<<competition-plurals-placeholder>>" },
        { nameof(Entities.COUNTRY), "<<country-placeholder>>" },
        { nameof(Entities.COUNTRIES), "<<country-plurals-placeholder>>" },
        { nameof(Entities.ENDURANCE_EVENT), "<<event-placeholder>>" },
        { nameof(Entities.ENDURANCE_EVENTS), "<<event-plurals-placeholder>>" },
        { nameof(Entities.HORSE), "<<horse-placeholder>>" },
        { nameof(Entities.HORSES), "<<horse-plurals-placeholder>>" },
        { nameof(Entities.PARTICIPANT), "<<participant-placeholder>>" },
        { nameof(Entities.PARTICIPANTS), "<<participant-plurals-placeholder>>" },
        { nameof(Entities.PARTICIPATION), "<<participation-placeholder>>" },
        { nameof(Entities.PARTICIPATIONS), "<<participation-plurals-placeholder>>" },
        { nameof(Entities.PERFORMANCE), "<<performance-placeholder>>" },
        { nameof(Entities.PERFORMANCES), "<<performance-plurals-placeholder>>" },
        { nameof(Entities.PERSONNEL), "<<personnel-placeholder>>" },
        { nameof(Entities.RESULT), "<<result-placeholder>>" },
        { nameof(Entities.RESULTS), "<<result-plurals-placeholder>>" },
        { nameof(Entities.PHASE), "<<phase-placeholder>>" },
        { nameof(Entities.PHASES), "<<phase-plurals-placeholder>>" },
    };
}
