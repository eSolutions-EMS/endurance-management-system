namespace EMS.Domain.Setup.Objects;

public record NumberCoreIdentifier : CoreIdentifier
{
    public NumberCoreIdentifier(int number)
    {
        this.Number = number;
    }
}
