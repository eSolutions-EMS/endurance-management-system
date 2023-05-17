namespace Core.Domain.State;

public interface IStateContext
{
    IState State { get; }
}
