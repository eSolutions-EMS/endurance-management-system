using EMS.Core.Application.Core;
using EMS.Core.Domain.State;
using EMS.Core.Domain.State.Competitions;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Core.Application.Queries;

public class CompetitionQueries : QueriesBase<Competition>
{
    public CompetitionQueries(IStateContext context) : base(context)
    {
    }
    protected override List<Competition> Set => this.State.Event.Competitions.ToList();
}
