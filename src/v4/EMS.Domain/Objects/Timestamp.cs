using Common.Domain;
using System.Globalization;

namespace EMS.Domain;

public record Timestamp(DateTime Time) : DomainObject
{
    public string ToString(string format, IFormatProvider formatProvider)
    {
        return this.Time.ToString(format, formatProvider);
    }
    public override string ToString()
    {
        return this.Time.ToString("HH:mm:ss.fff");
    }
}
