using EMS.Application;
using EMS.Domain.Objects;

namespace EMS.Judge.Ports;

public interface IJudgeContext : IUiContext
{
    IReadOnlyList<Country> Countries { get; }
}
