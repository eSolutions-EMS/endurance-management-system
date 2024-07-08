using Not;

namespace NTS.Judge.MAUI.Server.ACL.EMS;

public interface IEmsCompetitionState : IIdentifiable
{
    EmsCompetitionType Type { get; }
    string Name { get; }
    DateTime StartTime { get; }
    string FeiCategoryEventNumber { get; }
    string FeiScheduleNumber { get; }
    string Rule { get; }
    string EventCode { get; }
}
