using NTS.Compabitility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Countries;

public interface ICountryState : IIdentifiable
{
    string IsoCode { get; }

    string Name { get; }
}
