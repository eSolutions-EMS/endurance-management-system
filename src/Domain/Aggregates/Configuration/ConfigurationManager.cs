using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.EnduranceEvents;
using static EnduranceJudge.Localization.DesktopStrings;

namespace EnduranceJudge.Domain.Aggregates.Configuration
{
    public class ConfigurationManager : ManagerObjectBase, IAggregateRoot
    {
        private readonly IState state;

        public ConfigurationManager()
        {
            this.state = StaticProvider.GetService<IState>();
            this.Competitions = new CompetitionsManager(this.state.Event.Competitions);
            this.Phases = new PhasesManager(this.state.Event.Competitions);
        }

        public void Update(string name, int countryId, string populatedPlace)
            => this.Validate<EnduranceEventException>(() =>
        {
            this.state.Event.Name = name.IsRequired(NAME);
            this.state.Event.PopulatedPlace = populatedPlace.IsRequired(POPULATED_PLACE);

            countryId.IsRequired(COUNTRY);
            var country = this.state.Countries.FindDomain(countryId);
            this.state.Event.Country = country;
        });

        public CompetitionsManager Competitions { get; }
        public PhasesManager Phases { get; }
    }
}
