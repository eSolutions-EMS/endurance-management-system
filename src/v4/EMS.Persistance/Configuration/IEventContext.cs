using EMS.Domain.Setup.Entities;

namespace EMS.Persistence.Configuration;

public interface IEventContext
{
    Event? Event { get; }
}
