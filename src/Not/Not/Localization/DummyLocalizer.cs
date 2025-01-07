namespace Not.Localization;

public class DummyLocalizer : LocalizerBase
{
    protected override string GetLocalizedValue(string key)
    {
        return key;
    }
}
