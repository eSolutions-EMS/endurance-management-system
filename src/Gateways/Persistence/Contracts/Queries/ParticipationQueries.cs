using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Gateways.Persistence.Core;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Gateways.Persistence.Contracts.Queries
{
    public class ParticipationQueries : QueriesBase<Participation>
    {
        public ParticipationQueries(IState state) : base(state)
        {
        }

        protected override List<Participation> Set => this.State.Participations.ToList();
    }
}
