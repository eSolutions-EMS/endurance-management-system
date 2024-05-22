using NTS.Compabitility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Competitions;

public interface ICompetitionState : IIdentifiable
{
    CompetitionType Type { get; }
    string Name { get; }
    DateTime StartTime { get; }
}
