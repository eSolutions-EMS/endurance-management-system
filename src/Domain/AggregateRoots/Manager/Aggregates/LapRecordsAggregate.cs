using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Validation;
using EnduranceJudge.Domain.State.Results;
using EnduranceJudge.Domain.State.LapRecords;
using System;
using static EnduranceJudge.Localization.Strings;
using static EnduranceJudge.Domain.DomainConstants.ErrorMessages;

namespace EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates;

public class LapRecordsAggregate : IAggregate
{
    /// <summary>
    /// Update was not performed.
    /// The Update sequence must continue in a new LapRecord
    /// </summary>
    private bool ContinueUpdateSequence => true;
    /// <summary>
    /// Update was performed in this record.
    /// End Update sequence.
    /// </summary>
    private bool StopUpdateSequence => false;

    private readonly Validator<LapRecordException> validator;

    internal LapRecordsAggregate(LapRecord record)
    {
        this.Record = record;
        this.validator = new Validator<LapRecordException>();
    }

    public LapRecord Record { get; }

    internal bool Update(DateTime time)
    {
        if (time == default)
        {
            throw new ArgumentException(ARGUMENT_DEFAULT_VALUE, nameof(time));
        }
        if (this.Record.ArrivalTime == null)
        {
            this.Arrive(time);
            return this.StopUpdateSequence;
        }
        if (this.Record.InspectionTime == null)
        {
            this.Inspect(time);
            this.Complete();
            return this.StopUpdateSequence;
        }
        if (this.Record.IsReInspectionRequired && !this.Record.ReInspectionTime.HasValue)
        {
            this.CompleteReInspection(time);
            return this.StopUpdateSequence;
        }
        return this.ContinueUpdateSequence;
    }

    internal void Disqualify(string reason)
    {
        this.Record.Result = new Result(ResultType.Disqualified, reason);
    }
    internal void FailToQualify(string reason)
    {
        this.Record.Result = new Result(ResultType.FailedToQualify, reason);
    }
    internal void Resign(string reason)
    {
        this.Record.Result = new Result(ResultType.Resigned, reason);
    }
    internal void ReInspection(bool isRequired)
    {
        this.Record.IsReInspectionRequired = isRequired;
    }
    internal void RequireInspection(bool isRequired)
    {
        if (this.Record.Lap.IsCompulsoryInspectionRequired)
        {
            throw Helper.Create<LapRecordException>(REQUIRED_INSPECTION_IS_NOT_ALLOWED_MESSAGE);
        }
        this.Record.IsRequiredInspectionRequired = isRequired;
    }
    internal void Edit(ILapRecordState state)
    {
        if (state.ArrivalTime.HasValue && this.Record.ArrivalTime != state.ArrivalTime)
        {
            if (this.Record.ArrivalTime == null)
            {
                throw Helper.Create<LapRecordException>(CANNOT_EDIT_PERFORMANCE_MESSAGE, ARRIVAL_TERM);
            }
            this.Arrive(state.ArrivalTime.Value);
        }
        if (state.InspectionTime.HasValue && this.Record.InspectionTime != state.InspectionTime)
        {
            if (this.Record.InspectionTime == null)
            {
                throw Helper.Create<LapRecordException>(CANNOT_EDIT_PERFORMANCE_MESSAGE, INSPECTION_TERM);
            }
            this.Inspect(state.InspectionTime.Value);
        }
        if (state.ReInspectionTime.HasValue && this.Record.ReInspectionTime != state.ReInspectionTime)
        {
            if (this.Record.ReInspectionTime == null)
            {
                throw Helper.Create<LapRecordException>(CANNOT_EDIT_PERFORMANCE_MESSAGE, RE_INSPECTION_TERM);
            }
            this.CompleteReInspection(state.ReInspectionTime.Value);
        }
    }

    internal void Arrive(DateTime time)
    {
        // time = FixDateForToday(time);
        this.validator.IsLaterThan(time, this.Record.StartTime, ARRIVAL_TERM);
        this.Record.ArrivalTime = time;
    }
    internal void Inspect(DateTime time)
    {
        // time = FixDateForToday(time);
        this.validator.IsLaterThan(time, this.Record.ArrivalTime, INSPECTION_TERM);
        this.Record.InspectionTime = time;
    }
    private void CompleteReInspection(DateTime time)
    {
        // time = FixDateForToday(time);
        this.validator.IsLaterThan(time, this.Record.InspectionTime, RE_INSPECTION_TERM);

        this.Record.ReInspectionTime = time;
    }
    private void Complete()
    {
        if (!this.Record.ArrivalTime.HasValue || !this.Record.InspectionTime.HasValue)
        {
            throw new Exception(PERFORMANCE_INVALID_COMPLETE);
        }
        this.Record.Result = new Result(ResultType.Successful);
    }

    // TODO: remove after testing lap
    private DateTime FixDateForToday(DateTime date)
    {
        var today = DateTime.Today;
        today = today.AddHours(date.Hour);
        today = today.AddMinutes(date.Minute);
        today = today.AddSeconds(date.Second);
        today = today.AddMilliseconds(date.Millisecond);
        return today;
    }
}
