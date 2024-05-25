namespace NTS.Compatibility.EMS.Abstractions;

public class Validator<T> where T : DomainExceptionBase, new()
{
    public TValue IsRequired<TValue>(TValue obj, string property)
    {
        if (obj?.Equals(default) ?? true)
        {
            throw Helper.Create<T>("This is required {0}", property);
        }
        return obj;
    }

    public string IsFullName(string name)
    {
        this.IsRequired(name, "Name");
        var parts = name.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var firstName = parts.FirstOrDefault();
        var lastName = parts.LastOrDefault();
        if (parts.Length < 2 || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            throw Helper.Create<T>("Invalid full name {1}", name);
        }
        return name;
    }

    public void IsLaterThan(DateTime value, DateTime? compareTo, string name)
    {
        if (value <= compareTo)
        {
            throw Helper.Create<T>("Date has to be lated than {1}, {2}, {3}", name, value, compareTo);
        }
    }
}
