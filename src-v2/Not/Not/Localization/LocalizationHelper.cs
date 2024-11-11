using Not.Injection;

namespace Not.Localization;

public static class LocalizationHelper
{
    static readonly ILocalizer _localizer = ServiceLocator.Get<ILocalizer>();

    public static string Get(params object[] args)
    {
        return _localizer.Get(args);
    }
}

public static class LocalizationExtensions
{
    public static string Localize(this string text)
    {
        return LocalizationHelper.Get(text);
    }

    public static IEnumerable<string> Localize(this IEnumerable<string> words)
    {
        foreach (var word in words)
        {
            yield return LocalizationHelper.Get(word);
        }
    }
}
