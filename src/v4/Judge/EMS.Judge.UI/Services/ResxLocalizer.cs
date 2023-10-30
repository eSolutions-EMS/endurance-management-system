using Common.Services;
using EMS.Judge.UI.Resources.Localization;
using Microsoft.Extensions.Localization;

namespace EMS.Judge.UI.Services;

public class ResxLocalizer : LocalizerBase
{
    private readonly IStringLocalizer<Strings> _stringLocalizer;

    public ResxLocalizer(IStringLocalizer<Strings> stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
    }

    protected override string GetLocalizedValue(string key)
    {
        return _stringLocalizer[key];
    }
}
