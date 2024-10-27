using Newtonsoft.Json;

namespace NTS.Domain.Objects;

public class Person : IComparable<Person>
{
    internal static string DELIMITER = " ";

    public static Person? Create(string? names)
    {
        if (string.IsNullOrWhiteSpace(names))
        {
            return null;
        }
        return new Person(names.Split(DELIMITER, StringSplitOptions.RemoveEmptyEntries));
    }

    [JsonConstructor]
    public Person(string[] names) 
    {
        Names = names;
    }

    public string[] Names { get; private set; } = [];

    public override string ToString()
	{
		return string.Join(DELIMITER, this.Names);
	}

    public int CompareTo(Person? other)
    {
        if (other == null) return 1;

        // First compare Athlete
        int athleteComparison = string.Compare(String.Join(DELIMITER, Names), String.Join(" ", other.Names), StringComparison.Ordinal);
        return athleteComparison;
    }

    public static implicit operator string(Person member)
    {
        return member.ToString();
    }
}
