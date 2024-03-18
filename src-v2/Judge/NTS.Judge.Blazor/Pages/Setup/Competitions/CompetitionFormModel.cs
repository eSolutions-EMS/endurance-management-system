using NTS.Domain;
using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Setup.Competitions;

public class CompetitionFormModel
{
    public CompetitionFormModel()
    {
    }

    public CompetitionFormModel(Competition competition)
    {
        this.Name = competition.Name;
        this.Type = competition.Type;
        this.Start = competition.StartTime;

        //CompetitionFormModel specific properties
        DateTime? startDay = competition.StartTime.DateTime;
        TimeSpan? startTime = competition.StartTime.DateTime.TimeOfDay;
        this.StartDay = startDay;
        this.StartTime = startTime;
    }
    public DateTimeOffset ToDateTimeOffSet(DateTime time)
    {
        DateTime timeWithSpecifiedKind = DateTime.SpecifyKind(time, DateTimeKind.Local);
        DateTimeOffset offsetTime = timeWithSpecifiedKind;

        return offsetTime;
    }
    public DateTimeOffset CombineStartDayAndTime(DateTime? startDay, TimeSpan? startTime)
    {
        DateTime start = startDay.GetValueOrDefault(DateTime.MinValue);
        DateTime startDayTime = start.Add((TimeSpan)startTime);
        DateTimeOffset startTimeOffset = this.ToDateTimeOffSet(startDayTime);
        return startTimeOffset;
    }
    public string Name { get; set; }
    public CompetitionType Type { get; set; }

    public DateTimeOffset Start { get; set; }
    public List<Loop> Loops { get; set; } = new List<Loop>();
    public List<Contestant> Contestants { get; set; } = new List<Contestant>();

    public DateTime? StartDay { get; set; } = DateTime.Today;
    public TimeSpan? StartTime { get; set; }
}
