using EnduranceJudge.Application.Aggregates.Configurations;
using EnduranceJudge.Application.Core;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Personnels;
using System.Collections.Generic;

namespace EnduranceJudge.Application.Queries;

public class PersonnelQueries : QueriesBase<Personnel>
{
    private List<Personnel> personnel;

    public PersonnelQueries(IStateContext context) : base(context)
    {
    }

    protected override List<Personnel> Set
        => this.personnel
            ?? (this.personnel = PersonnelAggregator.Aggregate(this.State.Event));
}
