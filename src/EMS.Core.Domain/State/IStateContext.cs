namespace EnduranceJudge.Domain.State;

public interface IStateContext
{
    IState State { get; }
}
