using EnduranceJudge.Application.Core;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Horses;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Application.Queries;

public class HorseQueries : QueriesBase<Horse>
{
    public HorseQueries(IStateContext context) : base(context)
    {
    }

    protected override List<Horse> Set => this.State.Horses.ToList();
}
