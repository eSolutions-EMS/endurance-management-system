using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.State.Countries;
using System;

namespace EnduranceJudge.Domain.State.Athletes;

public class Athlete : DomainBase<AthleteException>, IAthleteState
{
    private const int ADULT_AGE_IN_YEARS = 18;

    private Athlete() {}
    public Athlete(string feiId, string firstName, string lastName, Country country, DateTime birthDate)
        : base(GENERATE_ID)
    {
        this.FeiId = feiId;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Country = country;
        this.Category = birthDate.AddYears(ADULT_AGE_IN_YEARS) <= DateTime.Now
            ? Category.Adults
            : Category.Kids;
    }
    public Athlete(IAthleteState state, Country country) : base(GENERATE_ID)
    {
        this.FeiId = state.FeiId;
        this.Club = state.Club;
        this.FirstName = state.FirstName;
        this.LastName = state.LastName;
        this.Category = state.Category;
        this.Country = country;
    }

    public string FeiId { get; internal set; }
    public string FirstName { get; internal set; }
    public string LastName { get; internal set; }
    public string Club { get; internal set; }
    public Category Category { get; internal set; }
    public Country Country { get; internal set; }

    public string Name => $"{this.FirstName} {this.LastName}";
}