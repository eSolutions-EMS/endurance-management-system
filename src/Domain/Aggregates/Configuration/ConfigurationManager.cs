using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.EnduranceEvents;
using static EnduranceJudge.Localization.DesktopStrings;

namespace EnduranceJudge.Domain.Aggregates.Configuration
{
    public class ConfigurationManager : ManagerObjectBase, IAggregateRoot
    {
        private EnduranceEvent enduranceEvent;
        private int countryId;

        public void Update(string name, int countryId, string populatedPlace)
            => this.Validate<EnduranceEventException>(() =>
        {
            name.IsRequired(NAME);
            populatedPlace.IsRequired(POPULATED_PLACE);
            this.countryId = countryId.IsRequired(COUNTRY);

            this.enduranceEvent = new EnduranceEvent(name, null, populatedPlace);
        });

        public void UpdateState(IState state)
        {
            var country = state.Countries.Find(x => x.Id == this.countryId);
            this.enduranceEvent.Country = country;
            state.Event.MapFrom(this.enduranceEvent);
        }
    }
}
