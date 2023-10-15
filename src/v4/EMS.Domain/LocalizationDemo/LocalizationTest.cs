namespace EMS.Domain;

public class LocalizationTest : DomainEntity
{
    public string Success()
        => "My dick Yanko";

    public string Invalid()
        => throw new DomainException("Kur {0}", "debel");
}
