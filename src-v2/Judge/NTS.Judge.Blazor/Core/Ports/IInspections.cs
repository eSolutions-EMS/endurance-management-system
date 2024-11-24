namespace NTS.Judge.Blazor.Core.Ports;

public interface IInspections : IParticipationContext
{
    Task RequestRepresent(bool requestFlag);
    Task RequireInspection(bool requestFlag);
}
