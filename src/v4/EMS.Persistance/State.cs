using Common.Conventions;
using EMS.Domain.Setup.Entities;

namespace EMS.Persistence;

public class State : IState
{
    public Event? Event { get; set; }
    public List<Official> StaffMembers { get; } = new();
}

public interface IState : ISingletonService
{
    Event? Event { get; set; }
    List<Official> StaffMembers { get; }
}