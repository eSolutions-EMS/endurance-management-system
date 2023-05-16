using EMS.Core.Application.Core;
using EMS.Core.Domain.State;
using EMS.Core.Domain.State.Horses;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Core.Application.Queries;

public class HorseQueries : QueriesBase<Horse>
{
    public HorseQueries(IStateContext context) : base(context)
    {
    }

    protected override List<Horse> Set => this.State.Horses.ToList();
}
