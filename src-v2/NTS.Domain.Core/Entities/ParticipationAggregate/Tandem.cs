using Newtonsoft.Json;

namespace NTS.Domain.Core.Entities.ParticipationAggregate;

public class Tandem : DomainEntity
{
    private readonly decimal _distance;

    [JsonConstructor]
    public Tandem(string number, Person name, decimal distance, decimal? gate, Country? country, Club? club)
    {
        Number = number;
        Name = name;
        _distance = distance;
        _gate = gate ?? 0;
        Country = country;
        Club = club;
    }

    public string Number { get; }
    public Person Name { get; }
    public string Distance => _distance.ToString("0.00");
    public Country? Country { get; }
    public Club? Club { get; }
}
