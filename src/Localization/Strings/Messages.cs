// ReSharper disable InconsistentNaming
namespace EnduranceJudge.Localization.Translations;

public static class Messages
{
    public static class DomainValidation
    {
        public static string CANNOT_REMOVE_USED_IN_PARTICIPANT                { get; internal set; }
        public static string ALREADY_PARTICIPATING_TEMPLATE                   { get; internal set; }
        public static string CANNOT_ADD_PARTICIPATION_DIFFERENT_PHASE_COUNT   { get; internal set; }
        public static string CANNOT_ADD_PARTICIPATION_DIFFERENT_PHASE_LENGTHS { get; internal set; }
        public static string DATE_TIME_HAS_TO_BE_LATER_TEMPLATE               { get; internal set; }
        public static string CANNOT_EDIT_PERFORMANCE                          { get; internal set; }
        public static string IS_REQUIRED_TEMPLATE                             { get; internal set; }
        public static string PARTICIPANT_NUMBER_NOT_FOUND_TEMPLATE            { get; internal set; }
        public static string CHANGE_NOT_ALLOWED_WHEN_EVENT_HAS_STARTED        { get; internal set; }
        public static string REQUIRED_INSPECTION_IS_NOT_ALLOWED               { get; internal set; }
        public static string INVALID_ORDER_BY                                 { get; internal set; }
        public static string INVALID_COMPETITION_NO_FINAL_PHASE               { get; internal set; }
        public static string INVALID_PARTICIPANT_NO_PARTICIPATIONS            { get; internal set; }
        public static string INVALID_PARTICIPANT_NO_COUNTRY                   { get; internal set; }
        public static string PARTICIPANT_HAS_NO_ACTIVE_PERFORMANCE            { get; internal set; }
        public static string INVALID_FULL_NAME                                { get; internal set; }
    }

    public static string SELECT_WORK_DIRECTORY            { get; internal set; }
    public static string REMOVE_CONFIRMATION_MESSAGE      { get; internal set; }
    public static string INVALID_DATE_FORMAT              { get; internal set; }
    public static string UNSUPPORTED_IMPORT_FILE_TEMPLATE { get; internal set; }
}
