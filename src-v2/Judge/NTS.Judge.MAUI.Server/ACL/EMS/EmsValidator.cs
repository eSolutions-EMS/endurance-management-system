namespace NTS.Judge.MAUI.Server.ACL.EMS;

public class EmsValidator<T> where T : EmsDomainExceptionBase, new()
{
    public TValue IsRequired<TValue>(TValue obj, string property)
    {
        if (obj?.Equals(default) ?? true)
        {
            throw EmsHelper.Create<T>("Value is required", property);
        }
        return obj;
    }

    public string IsFullName(string name)
    {
        IsRequired(name, "Name");
        var parts = name.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var firstName = parts.FirstOrDefault();
        var lastName = parts.LastOrDefault();
        if (parts.Length < 2 || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            throw EmsHelper.Create<T>("Full Name '{0}' is INVALID. Please use First and Last name", name);
        }
        return name;
    }

    public void IsLaterThan(DateTime value, DateTime? compareTo, string name)
    {
        if (value <= compareTo)
        {
            throw EmsHelper.Create<T>("invalid '{0}' with '{1}'. Has to be later than '{2}'", name, value, compareTo);
        }
    }
}
