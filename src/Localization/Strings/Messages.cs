// ReSharper disable InconsistentNaming
namespace EnduranceJudge.Localization.Strings;

public static class Messages
{
    public static class DomainValidation
    {
        public static string CANNOT_REMOVE_USED_IN_PARTICIPANT_MESSAGE                { get; internal set; }
        public static string ALREADY_PARTICIPATING_MESSAGE                            { get; internal set; }
        public static string CANNOT_ADD_PARTICIPATION_DIFFERENT_PHASE_COUNT_MESSAGE   { get; internal set; }
        public static string CANNOT_ADD_PARTICIPATION_DIFFERENT_PHASE_LENGTHS_MESSAGE { get; internal set; }
        public static string DATE_TIME_HAS_TO_BE_LATER_MESSAGE                        { get; internal set; }
        public static string CANNOT_EDIT_PERFORMANCE_MESSAGE                          { get; internal set; }
        public static string IS_REQUIRED_MESSAGE                                      { get; internal set; }
        public static string PARTICIPANT_NUMBER_NOT_FOUND_MESSAGE                     { get; internal set; }
        public static string CHANGE_NOT_ALLOWED_WHEN_EVENT_HAS_STARTED_MESSAGE        { get; internal set; }
        public static string REQUIRED_INSPECTION_IS_NOT_ALLOWED_MESSAGE               { get; internal set; }
        public static string INVALID_ORDER_BY_MESSAGE                                 { get; internal set; }
        public static string INVALID_COMPETITION_NO_FINAL_PHASE_MESSAGE               { get; internal set; }
        public static string INVALID_PARTICIPANT_NO_PARTICIPATIONS_MESSAGE            { get; internal set; }
        public static string INVALID_PARTICIPANT_NO_COUNTRY_MESSAGE                   { get; internal set; }
        public static string PARTICIPANT_HAS_NO_ACTIVE_PERFORMANCE_MESSAGE            { get; internal set; }
        public static string INVALID_FULL_NAME_MESSAGE                                { get; internal set; }
    }

    public static string SELECT_WORK_DIRECTORY_MESSAGE            { get; internal set; }
    public static string REMOVE_CONFIRMATION_MESSAGE              { get; internal set; }
    public static string INVALID_DATE_FORMAT_MESSAGE              { get; internal set; }
    public static string UNSUPPORTED_IMPORT_FILE_MESSAGE          { get; internal set; }
}
