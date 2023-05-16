using EMS.Core.Application.State;
using EMS.Core.Domain.State;

namespace EMS.Core.Application.Services;

public interface IStateSetter : IState
{
    internal void Set(StateModel initial);
}
