using NTS.Compatibility.EMS.Abstractions;
using NTS.Compatibility.EMS.Entities.Competitions;

namespace NTS.Compatibility.EMS.Entities.Competitions;

public interface IEmsCompetitionState : IEmsIdentifiable
{
    EmsCompetitionType Type { get; }
    string Name { get; }
    DateTime StartTime { get; }
    string FeiCategoryEventNumber { get; }
    string FeiScheduleNumber { get; }
    string Rule { get; }
    string EventCode { get; }
}
