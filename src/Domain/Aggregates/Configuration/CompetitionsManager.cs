using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.Validation;
using static EnduranceJudge.Localization.DesktopStrings;

namespace EnduranceJudge.Domain.Aggregates.Configuration
{
    public class CompetitionsManager : ManagerObjectBase
    {
        private readonly IState state;

        internal CompetitionsManager(IState state)
        {
            this.state = state;
        }

        public Competition Save(ICompetitionState state)
        {
            this.Validate<CompetitionException>(() =>
            {
                state.Type.IsRequired(TYPE);
                state.Name.IsRequired(NAME);
                state.StartTime.IsRequired(START_TIME).IsFutureDate();
            });

            var competition = this.state.Event.Competitions.FindDomain(state.Id);
            if (competition == null)
            {
                competition = new Competition(state);
                this.state.Event.Save(competition);
            }
            else
            {
                competition.Name = state.Name;
                competition.Type = state.Type;
                competition.StartTime = state.StartTime;
            }

            return competition;
        }
    }
}
