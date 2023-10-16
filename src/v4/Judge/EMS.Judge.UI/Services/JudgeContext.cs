using EMS.Domain.Objects;
using EMS.Judge.Ports;

namespace EMS.Judge.UI.Services;

public class JudgeContext : IJudgeContext
{
    private readonly List<Country> _countries;
    public JudgeContext()
    {
        this._countries = new()
        {
            new Country("BG", "Bulgaria"),
            new Country("US", "United States of America"),
            new Country("GB", "Great Britain"),
        };
    }

    public IReadOnlyList<Country> Countries => this._countries.AsReadOnly();
}
