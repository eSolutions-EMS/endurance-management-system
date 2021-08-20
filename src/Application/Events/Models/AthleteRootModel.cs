using EnduranceJudge.Application.Events.Queries.GetCountriesList;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using System.Collections.Generic;

namespace EnduranceJudge.Application.Events.Models
{
    public class AthleteRootModel : IAthleteState
    {
        public int Id { get; set; }
        public string FeiId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CountryIsoCode { get; set; }
        public Category Category { get; set; }
        public IEnumerable<CountryListModel> Countries { get; set; }
    }
}
