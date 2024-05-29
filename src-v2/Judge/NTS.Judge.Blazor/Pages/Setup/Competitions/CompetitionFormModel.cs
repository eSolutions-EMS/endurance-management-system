using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Setup.Competitions;

public class CompetitionFormModel
{

    public CompetitionFormModel()
    {
        //TODO: remove mock default values for testing
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
        CRIRecovery = competition.CriRecovery;
        UseAutomaticCRI = competition.CriRecovery != null;
    }

    public int? Id { get; set; }
    public string Name { get; set; }
    public CompetitionType Type { get; set; }
    public DateTime? StartDay { get; set; } = DateTime.Today;
    public TimeSpan? StartTime { get; set; }
<<<<<<< HEAD
    public IReadOnlyCollection<Phase>? Phases { get; }
=======
    public int? CRIRecovery { get; set; }
    public bool UseAutomaticCRI { get; set; }
    public IReadOnlyCollection<Loop>? Loops { get; }
>>>>>>> 111b808b6a976abe8041c24bb4a31807bff9b91f
    public IReadOnlyCollection<Contestant>? Contestants { get; }
}
 