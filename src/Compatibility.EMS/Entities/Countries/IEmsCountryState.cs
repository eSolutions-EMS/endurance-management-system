using NTS.Compatibility.EMS.Abstractions;

namespace Core.Domain.State.Countries;

public interface IEmsCountryState : IEmsIdentifiable
{
    string IsoCode { get; }

    string Name { get; }
}
