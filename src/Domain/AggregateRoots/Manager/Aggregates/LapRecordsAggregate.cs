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
    public bool IsComplete =>
        this.Record.ArrivalTime != null && this.Record.InspectionTime != null && !this.Record.IsReinspectionRequired
        || this.Record.ArrivalTime != null && this.Record.ReInspectionTime != null && this.Record.IsReinspectionRequired;

    internal void Vet(DateTime time)
    {
        if (this.Record.InspectionTime == null)
        {
            this.EnterIn(time);
        }
        else if (this.Record.ReInspectionTime == null && this.Record.IsReinspectionRequired)
        {
            this.EnterReIn(time);
        }
        this.CheckForResult();
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
    internal void RequireReInspection(bool isRequired)
    {
        this.Record.IsReinspectionRequired = isRequired;
    }
    internal void RequireCompulsoryInspection(bool isRequired)
    {
        if (this.Record.Lap.IsCompulsoryInspectionRequired)
        {
            throw Helper.Create<LapRecordException>(REQUIRED_INSPECTION_IS_NOT_ALLOWED_MESSAGE);
        }
        this.Record.IsRequiredInspectionRequired = isRequired;
    }
    internal void Edit(ILapRecordState state)
    {
        this.Record.ArrivalTime = state.ArrivalTime;
        this.Record.InspectionTime = state.InspectionTime;
        this.Record.ReInspectionTime = state.ReInspectionTime;
        if (state.ReInspectionTime.HasValue && this.HasRecovered(state.ReInspectionTime.Value)
            || state.InspectionTime.HasValue && this.HasRecovered(state.InspectionTime.Value))
        {
            this.Disqualify("Failed to Recover");
        }
    }

    internal void Arrive(DateTime time)
    {
        // time = FixDateForToday(time);
        this.validator.IsLaterThan(time, this.Record.StartTime, ARRIVAL_TERM);
        this.Record.ArrivalTime = time;
        this.CheckForResult();
    }
    private void EnterIn(DateTime time)
    {
        // time = FixDateForToday(time);
        this.validator.IsLaterThan(time, this.Record.ArrivalTime, INSPECTION_TERM);
        this.Record.InspectionTime = time;
    }
    private void EnterReIn(DateTime time)
    {
        // time = FixDateForToday(time);
        this.validator.IsLaterThan(time, this.Record.InspectionTime, RE_INSPECTION_TERM);
        this.Record.ReInspectionTime = time;
    }

    private bool HasRecovered(DateTime inTime)
    {
        var recoverySpan = TimeSpan.FromMinutes(this.Record.Lap.MaxRecoveryTimeInMins);
        return inTime - this.Record.ArrivalTime <= recoverySpan;
    }

    private void CheckForResult()
    {
        if (!this.Record.ArrivalTime.HasValue || !this.Record.InspectionTime.HasValue)
        {
            return;
        }
        var isRecoveredReInspection = this.Record.IsReinspectionRequired
            && this.Record.ReInspectionTime.HasValue
            && this.HasRecovered(this.Record.ReInspectionTime.Value);
        var isRecoveredInspection = !this.Record.IsReinspectionRequired
            && this.Record.InspectionTime.HasValue
            && this.HasRecovered(this.Record.InspectionTime.Value);
        if (isRecoveredReInspection || isRecoveredInspection)
        {
            this.Record.Result = new Result(ResultType.Successful);
        }
        else
        {
            this.FailToQualify("FTQ MET");
        }
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
