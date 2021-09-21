using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Application.Events.Queries.GetCountriesList;
using EnduranceJudge.Core.Models;
using EnduranceJudge.Domain.Aggregates.Event.EnduranceEvents;
using EnduranceJudge.Domain.States;
using System.Collections.Generic;

namespace EnduranceJudge.Application.Events.Queries.GetEvent
{
    public class EnduranceEventRootModel : IEnduranceEventState, IListable
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
