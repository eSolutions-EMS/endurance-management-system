using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Setup.Competitions;

public class CompetitionFormModel
{
    public CompetitionFormModel()
    {
        //mock default values for testing
        Name = "Olympic Games";
        Type = CompetitionType.FEI;
        StartDay = DateTime.Now;
        StartTime = DateTime.Now.TimeOfDay;
    }
    public CompetitionFormModel(Competition competition)
    {
        Id = competition.Id;
        Name = competition.Name;
        Type = competition.Type;
        DateTime? startDay = competition.StartTime.DateTime;
        TimeSpan? startTime = competition.StartTime.DateTime.TimeOfDay;
        StartDay = startDay;
        StartTime = startTime;
        Phases = competition.Phases;
        Contestants = competition.Contestants;
    }

    public int? Id { get; set; }
    public string Name { get; set; }
    public CompetitionType Type { get; set; }
    public DateTime? StartDay { get; set; } = DateTime.Today;
    public TimeSpan? StartTime { get; set; }
    public IReadOnlyCollection<Phase>? Phases { get; }
    public IReadOnlyCollection<Contestant>? Contestants { get; }
}
 