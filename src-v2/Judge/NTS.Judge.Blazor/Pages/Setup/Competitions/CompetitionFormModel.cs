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
        this.Start = competition.StartTime.DateTime;
        this.Loops = competition.Loops; 
        this.Contestants = competition.Contestants;
    }
    public DateTimeOffset ToDateTimeOffSet(DateTime time)
    {
        DateTime localTime1 = DateTime.SpecifyKind(time, DateTimeKind.Local);
        DateTimeOffset localTime2 = localTime1;

        return localTime2;
    }
    public string Name { get; set; }
    public CompetitionType Type { get; set; }
    public DateTime? Start { get; set; }
    public List<Loop> Loops { get; set; } = new List<Loop>();
    public List<Contestant> Contestants { get; set; } = new List<Contestant>();
}
