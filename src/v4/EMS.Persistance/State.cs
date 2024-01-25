using Common.Conventions;
using EMS.Domain.Setup.Entities;

namespace EMS.Persistence;

public class State : IState
{
    public Event? Event { get; set; }
    public List<Official> Officials  => Event?.Officials.ToList() ?? new List<Official>();
}

public interface IState : ISingletonService
{
    Event? Event { get; set; }
    List<Official> Officials { get; }
}