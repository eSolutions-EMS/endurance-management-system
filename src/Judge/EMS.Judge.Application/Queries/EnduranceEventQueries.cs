using EMS.Core.Domain.State;
using EMS.Core.Domain.State.EnduranceEvents;
using EMS.Judge.Application.Core;
using System;
using System.Collections.Generic;

namespace EMS.Judge.Application.Queries;

public class EnduranceEventQueries : IEnduranceEventQuery
{
    private readonly IState state;
    public EnduranceEventQueries(IStateContext context)
    {
        this.state = context.State;
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
