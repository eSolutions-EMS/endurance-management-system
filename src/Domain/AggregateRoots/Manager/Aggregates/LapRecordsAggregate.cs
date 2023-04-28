using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.Validation;
using EnduranceJudge.Domain.State.Results;
using EnduranceJudge.Domain.State.LapRecords;
using System;
using static EnduranceJudge.Localization.Strings;

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
        var inTime = state.InspectionTime;
        var reInTime = state.ReInspectionTime;
        this.Record.ArrivalTime = state.ArrivalTime;
        this.Record.InspectionTime = inTime;
        this.Record.ReInspectionTime = reInTime;
    }

    internal void Arrive(DateTime time)
    {
        // time = FixDateForToday(time);
        this.validator.IsLaterThan(time, this.Record.StartTime, ARRIVAL_TERM);
        this.Record.ArrivalTime = time;
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

    internal void CheckForResult(double? averageSpeedLimit, CompetitionType type)
    {
        if (!this.Record.ArrivalTime.HasValue || !this.Record.InspectionTime.HasValue)
        {
            return;
        }
        if (averageSpeedLimit.HasValue && Performance.GetSpeed(this.Record, type) > averageSpeedLimit)
        {
            this.Disqualify("Overtime");
            return;
        }
        var vetTime = this.Record.InspectionTime;
        if (this.Record.IsReinspectionRequired)
        {
            if (!this.Record.ReInspectionTime.HasValue)
            {
                return;
            }
            vetTime = this.Record.ReInspectionTime;
        }
        if (!this.HasRecovered(vetTime.Value))
        {
            this.FailToQualify("MET");
        }
        else
        {
            this.Record.Result = new Result(ResultType.Successful);
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
