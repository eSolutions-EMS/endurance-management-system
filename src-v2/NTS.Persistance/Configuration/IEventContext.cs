using NTS.Domain.Setup.Entities;

namespace NTS.Persistence.Configuration;

public interface IEventContext
{
    Event? Event { get; }
}
