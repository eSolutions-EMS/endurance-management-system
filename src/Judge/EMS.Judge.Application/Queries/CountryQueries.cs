using Core.Domain.State;
using Core.Domain.State.Countries;
using EMS.Judge.Application.Common;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Judge.Application.Queries;

public class CountryQueries : QueriesBase<Country>
{
    public CountryQueries(IStateContext context) : base(context)
    {
    }

    protected override List<Country> Set => this.State.Countries.ToList();
}
