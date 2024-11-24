namespace NTS.Judge.Blazor.Core.Ports;

public interface IInspectionsBehind : IParticipationContext
{
    Task RequestRepresent(bool requestFlag);
    Task RequireInspection(bool requestFlag);
}
