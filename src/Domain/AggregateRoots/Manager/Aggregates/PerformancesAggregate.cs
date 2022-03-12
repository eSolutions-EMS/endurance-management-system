using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Validation;
using EnduranceJudge.Domain.State.Results;
using EnduranceJudge.Domain.State.TimeRecords;
using System;
using static EnduranceJudge.Localization.Strings;
using static EnduranceJudge.Domain.DomainConstants.ErrorMessages;

namespace EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates
{
    // TODO: Rename to TimeRecordsAggregate
    public class PerformancesAggregate : IAggregate
    {
        private readonly LapRecord record;
        private readonly Validator<LapRecordException> validator;

        internal PerformancesAggregate(LapRecord record)
        {
            this.record = record;
            this.validator = new Validator<LapRecordException>();
        }

        internal void Update(DateTime time)
        {
            // TODO: Probably throw error as well
            this.validator.IsRequired(time, nameof(time));

            if (this.record.ArrivalTime == null)
            {
                this.Arrive(time);
            }
            else if (this.record.InspectionTime == null)
            {
                this.Inspect(time);
                if (!this.record.IsReInspectionRequired)
                {
                    this.Complete();
                }
            }
            else if (this.record.IsReInspectionRequired && this.record.ReInspectionTime == null)
            {
                this.CompleteReInspection(time);
                this.Complete();
            }
        }
        internal void Complete(string code)
        {
            this.record.Result = new Result(code);
        }
        internal void ReInspection(bool isRequired)
        {
            this.record.IsReInspectionRequired = isRequired;
        }
        internal void RequireInspection(bool isRequired)
        {
            if (this.record.Lap.IsCompulsoryInspectionRequired)
            {
                throw Helper.Create<LapRecordException>(REQUIRED_INSPECTION_IS_NOT_ALLOWED_MESSAGE);
            }
            this.record.IsRequiredInspectionRequired = isRequired;
        }
        internal void Edit(ILapRecordState state)
        {
            if (state.ArrivalTime.HasValue && this.record.ArrivalTime != state.ArrivalTime)
            {
                if (this.record.ArrivalTime == null)
                {
                    throw Helper.Create<LapRecordException>(CANNOT_EDIT_PERFORMANCE_MESSAGE, ARRIVAL_TERM);
                }
                this.Arrive(state.ArrivalTime.Value);
            }
            if (state.InspectionTime.HasValue && this.record.InspectionTime != state.InspectionTime)
            {
                if (this.record.InspectionTime == null)
                {
                    throw Helper.Create<LapRecordException>(CANNOT_EDIT_PERFORMANCE_MESSAGE, INSPECTION_TERM);
                }
                this.Inspect(state.InspectionTime.Value);
            }
            if (state.ReInspectionTime.HasValue && this.record.ReInspectionTime != state.ReInspectionTime)
            {
                if (this.record.ReInspectionTime == null)
                {
                    throw Helper.Create<LapRecordException>(CANNOT_EDIT_PERFORMANCE_MESSAGE, RE_INSPECTION_TERM);
                }
                this.CompleteReInspection(state.ReInspectionTime.Value);
            }
        }

        private void Arrive(DateTime time)
        {
            time = FixDateForToday(time);
            this.validator.IsLaterThan(time, this.record.StartTime, ARRIVAL_TERM);
            this.record.ArrivalTime = time;
        }
        private void Inspect(DateTime time)
        {
            time = FixDateForToday(time);
            this.validator.IsLaterThan(time, this.record.ArrivalTime, INSPECTION_TERM);
            this.record.InspectionTime = time;
        }
        private void CompleteReInspection(DateTime time)
        {
            time = FixDateForToday(time);
            this.validator.IsLaterThan(time, this.record.InspectionTime, RE_INSPECTION_TERM);

            this.record.ReInspectionTime = time;
        }
        private void Complete()
        {
            if (!this.record.ArrivalTime.HasValue || !this.record.InspectionTime.HasValue)
            {
                throw new Exception(PERFORMANCE_INVALID_COMPLETE);
            }
            this.record.Result = new Result();
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
}
