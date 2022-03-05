namespace EnduranceJudge.Localization.Translations;

public static class Messages
{
    public static class DomainValidation
    {
        public const string CANNOT_REMOVE_USED_IN_PARTICIPANT = $"cannot be removed because it is used in an existing {Entities.PARTICIPANT}";
        public const string ALREADY_PARTICIPATING_TEMPLATE = "cannot be created - '{0}' is already participating.";
        public const string CANNOT_ADD_PARTICIPATION_DIFFERENT_PHASE_COUNT = $"cannot be added, because they already participate in '{{0}}' with different amount of {Entities.PHASE}s ";
        public const string CANNOT_ADD_PARTICIPATION_DIFFERENT_PHASE_LENGTHS = $"cannot be added in '{{0}}'- they already participate in '{{1}}' and difference is detected in '{Entities.PHASE} {{2}}' lengths: '{{3}}' and '{{4}}";
        public const string DATE_TIME_HAS_TO_BE_LATER_TEMPLATE = "{0} has to be later than {1}";
        public const string CANNOT_EDIT_PERFORMANCE = $"- INVALID edit. Value does not exist for '{{0}}'. 'Edit' button should only be used to correct errors. Please use '{Words.UPDATE}' button above when setting Times.";
        public const string IS_REQUIRED_TEMPLATE = "'{0}' is required.";
        public const string PARTICIPANT_NUMBER_NOT_FOUND_TEMPLATE = "'{0}' does not exit";
        public const string CHANGE_NOT_ALLOWED_WHEN_EVENT_HAS_STARTED = $"changes is not allowed after {Entities.PARTICIPATION}s have started";
        public const string REQUIRED_INSPECTION_IS_NOT_ALLOWED = $"'s Individual Required Inspection is not allowed, because CRI is enabled on this '{Entities.PHASE}'";
        public const string INVALID_ORDER_BY = "with order '{0}' already exists";
        public const string INVALID_COMPETITION_NO_FINAL_PHASE = $"{{0}} has no final {Entities.PHASE}";
        public const string INVALID_PARTICIPANT_NO_PARTICIPATIONS = $"'{{0}}' is not included in any {Entities.COMPETITION}";
        public const string INVALID_PARTICIPANT_NO_COUNTRY = $"'{{0}}' has no {Entities.COUNTRY}";
        public const string PARTICIPANT_HAS_NO_ACTIVE_PERFORMANCE = $"'{{0}}' has no active {Entities.PERFORMANCE}";
        public const string INVALID_FULL_NAME = "Full Name '{0}' is INVALID. Please use First and Last name";
    }

    public const string SELECT_WORK_DIRECTORY = $"Select working directory using the '{Words.IMPORT}' button above.";
    public const string REMOVE_CONFIRMATION_MESSAGE = "Are you sure you want to remove this item";
    public const string INVALID_DATE_FORMAT = "Invalid date: '{0}'. Expected format: '{1}'.";
    public const string UNSUPPORTED_IMPORT_FILE_TEMPLATE =  "Unsupported file. Please use '{0}' or '{1}'.";
}
