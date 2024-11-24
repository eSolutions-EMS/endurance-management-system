using Microsoft.Extensions.Localization;
using Not.Localization;

namespace NTS.Judge.Blazor.Shared.Services;

public class ResxLocalizer : LocalizerBase
{
    readonly IStringLocalizer<Strings> _stringLocalizer;

    public ResxLocalizer(IStringLocalizer<Strings> stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
    }

    protected override string GetLocalizedValue(string key)
    {
        return _stringLocalizer[key];
    }
}
