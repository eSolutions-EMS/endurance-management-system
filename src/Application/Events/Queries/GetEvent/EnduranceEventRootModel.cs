using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Application.Events.Queries.GetCountriesList;
using EnduranceJudge.Domain.Aggregates.Event.EnduranceEvents;
using System.Collections.Generic;

namespace EnduranceJudge.Application.Events.Queries.GetEvent
{
    public class EnduranceEventRootModel : IEnduranceEventState
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PopulatedPlace { get; set; }
        public string CountryIsoCode { get; set; }
        public IEnumerable<CompetitionDependantModel> Competitions { get; set; }
        public IEnumerable<PersonnelDependantModel> Personnel { get; set; }
        public IEnumerable<CountryListModel> Countries { get; set; }
    }
}
