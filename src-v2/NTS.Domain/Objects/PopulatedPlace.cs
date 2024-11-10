namespace NTS.Domain.Objects;

public record PopulatedPlace : DomainObject
{
    readonly string _city;
    readonly string? _location;

    public PopulatedPlace(Country country, string city, string? location)
    {
        Country = country;
        _city = city;
        _location = location;
    }

    public Country Country { get; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        if (_location != null)
        {
            sb.Append($"{_location} ");
        }
        if (_city != null)
        {
            sb.Append($"{_city} ");
        }
        var country = Country.ToString();
        sb.Append(country);
        return sb.ToString();
    }
}
