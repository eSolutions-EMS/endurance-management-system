using System;

namespace EnduranceJudge.Localization.Translations;

public static class Messages
{
    public const string CANNOT_REMOVE_USED_IN_PARTICIPANT = "Cannot be remobed because it is used in an existing Participant";
    public const string ALREADY_PARTICIPATING_TEMPLATE = "Cannot be created - '{0}' is already participating.";
    public const string SELECT_WORK_DIRECTORY = "Select working directory using the 'Import' button above.";
    public const string CANNOT_ADD_PARTICIPATION_DIFFERENT_PHASE_COUNT = "Cannot add Participation, because this competitor already participates in '{0}' with different amount of Phases ";
    public const string CANNOT_ADD_PARTICIPATION_DIFFERENT_PHASE_LENGTHS = "Cannot add Participation in '{0}'- competitior already participates in '{1}' and difference is detected in 'Phase {2}' lengths: '{3}' and '{4}";
    public const string HAS_ALREADY_STARTED = "has already started";
    public const string PARTICIPANT_CANNOT_START_NO_COMPETITION_TEMPLATE = "Participant '{0}' cannot start, because they don't participate in any Competition";
    public const string CAN_ONLY_BE_COMPLETED = "can only be completed.";
    public const string DATE_TIME_HAS_TO_BE_LATER_TEMPLATE = "{0} has to be later than {1}";
    public const string CANNOT_START_COMPETITION_WITHOUT_PHASES = "cannot start - competition has no phases.";
    public const string REQUIRED_INSPECTION_IS_NULL_MESSAGE = "cannot complete: Required Inspection is required and is not completed";
    public const string COMPULSORY_REQUIRED_INSPECTION_IS_NULL_MESSAGE = "cannot complete: Compulsory Required Inspection is required and is not completed";
    public const string ARRIVAL_TIME_IS_NULL_MESSAGE = "cannot complete: ArrivalTime cannot be null.";
    public const string INSPECTION_TIME_IS_NULL_MESSAGE = "cannot complete: InspectionTime cannot be null";
    public const string CANNOT_START_NEXT_PERFORMANCE_PARTICIPATION_IS_COMPLETE = "cannot start next Performance. This Participant is already finished";
    public const string CANNOT_START_PERFORMANCE_NO_PHASE = "cannot start performance - no Phase found.";
    public const string CANNOT_START_PERFORMANCE_NO_START_TIME ="cannot start next performance - no Start Time.";
    public const string CANNOT_START_NEXT_PERFORMANCE_NO_LAST_PERFORMANCE ="Cannot start next Performance. Participations have not started or data is invalid.";
    public const string CANNOT_EDIT_PERFORMANCE = "Editting '{0}' is not allowed, becausue no value exists. Please use 'Update' button above.";
    public const string IS_REQUIRED_TEMPLATE = "property '{0}' is required.";
    public const string CANNOT_REMOVE_NULL_ITEM_TEMPLATE = "cannot remove 'null' from collection..";
    public const string CANNOT_REMOVE_ITEM_IS_NOT_FOUND_TEMPLATE = "cannot remove '{0}' - it is not found.";
    public const string CANNOT_ADD_NULL_ITEM_TEMPLATE = "cannot add 'null' to a collection.";
    public const string INVALID_FUTURE_DATE_TEMPLATE = "Date '{0}' is not future date.";
    public const string UNSUPPORTED_IMPORT_FILE_TEMPLATE =  "Unsupported file. Please use '{0}' or '{1}'.";
    public const string INVALID_DATE_FORMAT = "Invalid date: '{0}'. Expected format: '{1}'.";
    public const string REMOVE_CONFIRMATION_MESSAGE = "Are you sure you want to remove this item";
    public const string PARTICIPANT_NUMBER_NOT_FOUND_TEMPLATE = "Participant with number '{0}' not found.";
    public const string CHANGE_NOT_ALLOWED_WHEN_EVENT_HAS_STARTED = "This change is not allowed after Participations have started ";
    public const string REQUIRED_INSPECTION_IS_NOT_ALLOWED = "Individual Required Inspection is not allowed, because CRI is enabled on this Phase";
    public const string INVALID_COMPETITION_NO_FINAL_PHASE = $"Competition {{0}} has no final {Entities.PHASE}";
    public const string INVALID_ORDER_BY = "Phase with order {0} already exists.";
    public const string INVALID_PARTICIPANT_NO_PARTICIPATIONS = $"Participant {{0}} is not included in any {Entities.COMPETITION}";
}
