using Common.Conventions;

namespace EMS.Domain;

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

public interface ILocalizationTestService : ITransientService
{
    string Polite();
    string Rude();
}