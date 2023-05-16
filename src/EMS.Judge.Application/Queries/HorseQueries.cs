using EMS.Core.Domain.State;
using EMS.Core.Domain.State.Horses;
using EMS.Judge.Application.Core;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Judge.Application.Queries;

public class HorseQueries : QueriesBase<Horse>
{
    public HorseQueries(IStateContext context) : base(context)
    {
    }

    protected override List<Horse> Set => this.State.Horses.ToList();
}
