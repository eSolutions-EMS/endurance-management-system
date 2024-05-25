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
        StartTime = TimeSpan.Zero;
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
        Loops = competition.Loops;
        Contestants = competition.Contestants;
        CRIRecovery = competition.CriRecovery;
        UseAutomaticCRI = competition.CriRecovery != null;
    }

    public int? Id { get; set; }
    public string Name { get; set; }
    public CompetitionType Type { get; set; }
    public DateTime? StartDay { get; set; } = DateTime.Today;
    public TimeSpan? StartTime { get; set; }
    public int? CRIRecovery { get; set; }
    public bool UseAutomaticCRI { get; set; }
    public IReadOnlyCollection<Loop>? Loops { get; }
    public IReadOnlyCollection<Contestant>? Contestants { get; }
}
 