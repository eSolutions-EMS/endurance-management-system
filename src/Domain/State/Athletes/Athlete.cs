using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using System;

namespace EnduranceJudge.Domain.State.Athletes
{
    public class Athlete : DomainObjectBase<AthleteObjectException>, IAthleteState
    {
        private const int ADULT_AGE_IN_YEARS = 18;

        private Athlete() {}
        public Athlete(string feiId, string firstName, string lastName, string countryCode, DateTime birthDate)
            : this(feiId, firstName, lastName, countryCode, GetCategory(birthDate))
        {
        }

        public Athlete(string feiId, string firstName, string lastName, string countryCode, Category category)
            : base(true)
            => this.Validate(() =>
        {
            this.FeiId = feiId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.CountryIsoCode = countryCode;
            this.Category = category;
        });

        public string FeiId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Club { get; private set; }
        public Category Category { get; private set; }
        public string CountryIsoCode { get; private set; }

        private static Category GetCategory(DateTime birthDate)
        {
            var category = birthDate.AddYears(ADULT_AGE_IN_YEARS) <= DateTime.Now
                ? Category.Adults
                : Category.Kids;
            return category;
        }
    }
}
