using Common.Domain;
using System.Globalization;

namespace EMS.Domain;

public record Timestamp(DateTime Time) : DomainObject
{
    public override string ToString()
    {
        return this.Time.ToString("HH:mm:ss.fff");
    }
}
