using Common.Services;

namespace Common.Localization;

public static class Localizer
{
	private static readonly ILocalizer _localizer = ServiceLocator.Get<ILocalizer>();

	public static string Get(params object[] args)
	{
		return _localizer.Get(args);
	}

	public static string Localize(this string text)
	{
		return _localizer.Get(text);
	}

	public static string Localize(params object[] args)
	{
		return Get(args);
	}

    public static IEnumerable<string> LocalizeSeparately(IEnumerable<string> words)
    {
        foreach (var word in words)
        {
            yield return _localizer.Get(word);
        }
    }
}
