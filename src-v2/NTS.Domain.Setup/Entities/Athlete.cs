using Newtonsoft.Json;
using Not.Domain.Base;
using NTS.Domain.Setup.Import;

namespace NTS.Domain.Setup.Entities;

public class Athlete : DomainEntity, IImportable
{
    public static Athlete Create(
        string? name,
        string? feiId,
        Country? country,
        string? club,
        AthleteCategory? category
    )
    {
        return new(Person.Create(name), feiId, country, Club.Create(club), category);
    }

    public static Athlete Update(
        int id,
        string? name,
        string? feiId,
        Country? country,
        string? club,
        AthleteCategory? category
    )
    {
        return new(id, Person.Create(name), feiId, country, Club.Create(club), category);
    }

    [JsonConstructor]
    Athlete(
        int id,
        Person? person,
        string? feiId,
        Country? country,
        Club? club,
        AthleteCategory? category
    )
        : base(id)
    {
        FeiId = feiId;
        Person = Required(nameof(Person), person);
        Country = Required(nameof(Country), country);
        Club = Required(nameof(Club), club);
        Category = Required(nameof(Category), category);
    }

    Athlete( //TODO: consider Club as persisted across Events (MAUI's raw resources?)
        Person? person,
        string? feiId,
        Country? country,
        Club? club,
        AthleteCategory? category
    )
        : this(GenerateId(), person, feiId, country, club, category) { }

    public string? FeiId { get; }
    public Person Person { get; }
    public Country Country { get; }
    public Club Club { get; }
    public AthleteCategory Category { get; private set; }

    public override string ToString()
    {
        return Person.ToString();
    }
}
