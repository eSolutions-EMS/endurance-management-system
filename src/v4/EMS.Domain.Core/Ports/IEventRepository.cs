using Common.Conventions;

namespace EMS.Domain.Core.Ports;

public interface IEventRepository : ITransientService
{
    Task<Event> Get(Guid id);
}
