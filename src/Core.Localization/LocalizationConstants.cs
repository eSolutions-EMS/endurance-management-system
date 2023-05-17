using Core.Utilities;
using System.Reflection;

namespace Core.Localization;

public static class LocalizationConstants
{
    public const string DEFAULT_COUNTRY_CODE = "BGR";
    public const string STRINGS_FILENAME = "strings.txt";

    internal static class Placeholders
    {
        public const string ATHLETE =                  "{entities-athlete}";
        public const string ATHLETES =                 "{entities-athlete-plurals}";
        public const string COMPETITION =              "{entities-competition}";
        public const string COMPETITIONS =             "{entities-competition-plurals}";
        public const string COUNTRY =                  "{entities-country}";
        public const string COUNTRIES =                "{entities-country-plurals}";
        public const string EVENT =                    "{entities-event}";
        public const string EVENTS =                   "{entities-event-plurals}";
        public const string HORSE =                    "{entities-horse}";
        public const string HORSES =                   "{entities-horse-plurals}";
        public const string PARTICIPANT =              "{entities-participant}";
        public const string PARTICIPANTS =             "{entities-participant-plurals}";
        public const string PARTICIPATION =            "{entities-participation}";
        public const string PARTICIPATIONS =           "{entities-participation-plurals}";
        public const string PERFORMANCE =              "{entities-performance}";
        public const string PERFORMANCES =             "{entities-performance-plurals}";
        public const string PERSONNEL =                "{entities-personnel}";
        public const string RESULT =                   "{entities-result}";
        public const string RESULTS =                  "{entities-result-plurals}";
        public const string LAP =                      "{entities-lap}";
        public const string LAPS =                     "{entities-lap-plurals}";
        public const string WORDS_UPDATE =             "{words-update}";
        public const string WORDS_SELECT_DIRECTORY =   "{words-select-directory}";
        public const string WORDS_ORDER =              "{words-order-by}";
    };

    public static Assembly[] Assemblies
    {
        get
        {
            var assemblies = ReflectionUtilities.GetAssemblies("Core.Localization");
            return assemblies;
        }
    }
}
