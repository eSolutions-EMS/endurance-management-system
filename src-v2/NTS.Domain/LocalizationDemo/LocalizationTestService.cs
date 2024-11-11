using Not.Injection;

namespace NTS.Domain.LocalizationDemo;

public class LocalizationTestService : ILocalizationTestService
{
    public string Polite()
    {
        return new LocalizationTest().Success();
    }

    public string Rude()
    {
        return new LocalizationTest().Invalid();
    }
}

public interface ILocalizationTestService : ITransient
{
    string Polite();
    string Rude();
}
