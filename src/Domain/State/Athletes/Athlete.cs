using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using System;
using static EnduranceJudge.Localization.DesktopStrings;

namespace EnduranceJudge.Domain.State.Athletes
{
    public class Athlete : DomainObjectBase<RiderException>, IAthleteState, IAggregateRoot
    {
        private const int ADULT_AGE_IN_YEARS = 18;

        private Athlete() {}

        public Athlete(int id, string feiId, string firstName, string lastName, string countryCode, DateTime birthDate)
            : this(id, feiId, firstName, lastName, countryCode, GetCategory(birthDate))
        {
        }

        public Athlete(int id, string feiId, string firstName, string lastName, string countryCode, Category category)
            : base(id)
            => this.Validate(() =>
        {
            this.FeiId = feiId.IsRequired(FEI_ID);
            this.FirstName = firstName.IsRequired(FIRST_NAME);
            this.LastName = lastName.IsRequired(LAST_NAME);
            this.CountryIsoCode = countryCode.IsRequired(COUNTRY);
            this.Category = category.IsRequired(CATEGORY);
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
