using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Domain.Enums;

namespace EnduranceJudge.Application.Events.Models
{
    public class AthleteRootModel : IAthleteState
    {
        public int Id { get; set; }
        public string FeiId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Club { get; set; }
        public string CountryIsoCode { get; set; }
        public Category Category { get; set; }
    }
}
