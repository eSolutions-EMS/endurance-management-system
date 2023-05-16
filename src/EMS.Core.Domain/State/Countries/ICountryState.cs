using EMS.Core.Models;

namespace EMS.Core.Domain.State.Countries;

public interface ICountryState : IIdentifiable
{
    string IsoCode { get; }

    string Name { get; }
}
