using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using System;

namespace EnduranceJudge.Domain.Aggregates.Common.Athletes
{
    public class Athlete : DomainBase<RiderException>, IAthleteState
    {
        private const int AdultAgeInYears = 18;

        private Athlete()
        {
        }

        public Athlete(int id, string feiId, string firstName, string lastName, string countryCode, DateTime birthDate)
            : this(id, feiId, firstName, lastName, countryCode, GetCategory(birthDate))
        {
        }

        public Athlete(int id, string feiId, string firstName, string lastName, string countryCode, Category category)
            : base(id)
            => this.Validate(() =>
            {
                this.FeiId = feiId.IsRequired(nameof(feiId));
                this.FirstName = firstName.IsRequired(nameof(firstName));
                this.LastName = lastName.IsRequired(nameof(lastName));
                this.CountryIsoCode = countryCode.IsRequired(nameof(countryCode));
                this.Category = category.IsRequired(nameof(category));
            });

        public string FeiId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Category Category { get; private set; }
        public string CountryIsoCode { get; private set; }

        private static Category GetCategory(DateTime birthDate)
        {
            var category = birthDate.AddYears(AdultAgeInYears) <= DateTime.Now
                ? Category.Adults
                : Category.Kids;

            return category;
        }
    }
}
