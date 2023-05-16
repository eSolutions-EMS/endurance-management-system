using EMS.Core.Domain.State;
using EMS.Judge.Application.State;

namespace EMS.Judge.Application.Services;

public interface IStateSetter : IState
{
    internal void Set(StateModel initial);
}
