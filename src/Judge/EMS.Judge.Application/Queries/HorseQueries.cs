using System.Collections.Generic;
using System.Linq;
using Core.Domain.State;
using Core.Domain.State.Horses;
using EMS.Judge.Application.Common;

namespace EMS.Judge.Application.Queries;

public class HorseQueries : QueriesBase<Horse>
{
    public HorseQueries(IStateContext context)
        : base(context) { }

    protected override List<Horse> Set => this.State.Horses.ToList();
}
