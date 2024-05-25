using Core.Domain.Enums;
using NTS.Compabitility.EMS.Abstractions;
using NTS.Compatibility.EMS.Entities.Competitions;

namespace Core.Domain.State.Competitions;

public interface ICompetitionState : IIdentifiable
{
    CompetitionType Type { get; }
    string Name { get; }
    DateTime StartTime { get; }
    public string FeiId { get; }
    public string FeiScheduleNumber { get; }
    public string Rule { get; }
}
