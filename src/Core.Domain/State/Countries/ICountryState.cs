using Core.Models;

namespace Core.Domain.State.Countries;

public interface ICountryState : IIdentifiable
{
    string IsoCode { get; }

    string Name { get; }
}
