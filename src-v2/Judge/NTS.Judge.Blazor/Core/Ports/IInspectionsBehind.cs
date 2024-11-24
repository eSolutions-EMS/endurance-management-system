namespace NTS.Judge.Blazor.Core.Ports;

public interface IInspectionsBehind : ISelectedParticipationBehind
{
    Task RequestRepresent(bool requestFlag);
    Task RequireInspection(bool requestFlag);
}
