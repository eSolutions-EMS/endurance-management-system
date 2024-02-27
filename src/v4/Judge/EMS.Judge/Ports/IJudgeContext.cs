using NTS.Application;
using NTS.Domain.Objects;

namespace NTS.Judge.Ports;

public interface IJudgeContext : IUiContext
{
    IReadOnlyList<Country> Countries { get; }
}
