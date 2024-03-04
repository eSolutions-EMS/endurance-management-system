namespace NTS.Domain.Core.Objects;

public record NumberCoreIdentifier : CoreIdentifier
{
    public NumberCoreIdentifier(int Number)
    {
        this.Number = Number;
    }
}
