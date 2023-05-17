using Core.Domain.State;
using Core.Domain.State.Competitions;
using EMS.Judge.Application.Common;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Judge.Application.Queries;

public class CompetitionQueries : QueriesBase<Competition>
{
    public CompetitionQueries(IStateContext context) : base(context)
    {
    }
    protected override List<Competition> Set => this.State.Event.Competitions.ToList();
}
