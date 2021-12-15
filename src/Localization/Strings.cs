namespace EnduranceJudge.Localization
{
    public class Strings
    {
        public static class Domain
        {
            public const string IS_REQUIRED_TEMPLATE = "property '{0}' is required.";
            public const string CANNOT_REMOVE_NULL_ITEM_TEMPLATE = "cannot remove 'null' from collection..";
            public const string CANNOT_REMOVE_ITEM_IS_NOT_FOUND_TEMPLATE = "cannot remove '{0}' - it is not found.";
            public const string CANNOT_ADD_NULL_ITEM_TEMPLATE = "cannot add 'null' to a collection.";
            public const string CANNOT_ADD_ITEM_EXISTS_TEMPLATE = "cannot add '{0}' because entity with Id '{1}' already exists.";
            public const string INVALID_FUTURE_DATE_TEMPLATE = "Date '{0}' is not future date.";

            public static class Manager
            {
                public static class Participation
                {
                    public const string HAS_ALREADY_STARTED = "has already started";
                    public const string HAS_NOT_STARTED = "has not started yet";
                    public const string CAN_ONLY_BE_COMPLETED = "can only be completed.";
                    public const string CANNOT_BE_COMPLETED = "cannot be completed";
                    public const string DATE_TIME_HAS_TO_BE_LATER_TEMPLATE = "{0} has to be later than {1}";
                    public const string CANNOT_START_COMPETITION_WITHOUT_PHASES = "cannot start - competition has no phases.";
                    public const string REQUIRED_INSPECTION_IS_NULL_MESSAGE = "cannot complete: Additional Inspection is required and is not completed";
                    public const string ARRIVAL_TIME_IS_NULL_MESSAGE = "cannot complete: ArrivalTime cannot be null.";
                    public const string INSPECTION_TIME_IS_NULL_MESSAGE = "cannot complete: InspectionTime cannot be null";
                    public const string CANNOT_COMPLETE_REQUIRED_INSPECTION = "cannot complete : Aditional inspecion is not required. Tick the checkbox to require one";
                    public const string CANNOT_START_NEXT_PERFORMANCE_PARTICIPATION_IS_COMPLETE = "cannot start next Performance. This Participant is already finished";
                    public const string CANNOT_START_PERFORMANCE_NO_PHASE = "cannot start performance - no Phase found.";
                    public const string CANNOT_START_PERFORMANCE_NO_START_TIME ="cannot start next performance - no Start Time.";
                    public const string CANNOT_EDIT_PERFORMANCE = "Editting '{0}' is not allowed, becausue no value exists. Please use 'Update' button above.";
                }
            }

            public static class Ranking
            {
                public const string INCOMPLETE_PARTICIPATIONS = " is invalid - contains uncomplete competitions";
            }

            public static class Countries
            {
                public const string BULGARIA = "Bulgaria";
            }
        }

        public static class Application
        {
            public const string UNSUPPORTED_IMPORT_FILE_TEMPLATE =  "Unsupported file. Please use '{0}' or '{1}'.";
            public const string COMPETITIONS_WITH_DIFFERENT_START_TIMES = "Invalid configuration. Competitions have different starttimes.";
            public const string NO_COMPETITIONS = "There are no competitions";
            public const string INVALID_DATE_FORMAT = "Invalid date: '{0}'. Expected format: '{1}'.";
        }

        public static class DesktopStrings
        {
            public const string APPLICATION_ERROR = "Application Error";
            public const string VALIDATION_TITLE = "Validation Message";
            public const string CONFIRMATION_TITLE = "Confirmation";
            public const string REMOVE_CONFIRMATION_MESSAGE = "Are you sure you want to remove this item";
            public const string PARTICIPANT_NUMBER_NOT_FOUND_TEMPLATE = "Participant with number '{0}' not found.";
        }
    }
}
