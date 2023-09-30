using EMS.Domain;

namespace EMS.Domain.Core;

public class NumberCoreIdentifier : CoreIdentifier
{
    public NumberCoreIdentifier(int Number)
    {
        this.Number = Number;
    }
}
