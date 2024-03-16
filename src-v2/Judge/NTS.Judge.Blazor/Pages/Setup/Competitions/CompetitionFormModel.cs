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
        this.Loops = competition.Loops; 
        this.Contestants = competition.Contestants;
    }

    public DateTime GetStartTime()
    {
        return this.Start.DateTime;
    }

    public string Name { get; set; }
    public CompetitionType Type { get; set; }
    public DateTimeOffset Start { get; set; }
    public List<Loop> Loops { get; set; }
    public List<Contestant> Contestants { get; set; }
}
