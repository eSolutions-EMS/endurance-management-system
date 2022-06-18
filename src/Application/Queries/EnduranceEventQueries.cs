using EnduranceJudge.Application.Core;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.EnduranceEvents;
using System;
using System.Collections.Generic;

namespace EnduranceJudge.Application.Queries;

public class EnduranceEventQueries : IEnduranceEventQuery
{
    private readonly IState state;
    public EnduranceEventQueries(IState state)
    {
        this.state = state;
    }

    public EnduranceEvent Get()
    {
        return this.state.Event;
    }

    public EnduranceEvent GetOne(Predicate<EnduranceEvent> predicate) => throw new NotImplementedException();
    public EnduranceEvent GetOne(int id) => throw new NotImplementedException();
    public List<EnduranceEvent> GetAll() => throw new NotImplementedException();
}

public interface IEnduranceEventQuery : IQueries<EnduranceEvent>
{
    EnduranceEvent Get();
}
