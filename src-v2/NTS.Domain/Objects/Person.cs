using Newtonsoft.Json;
using System.Xml.Linq;

namespace NTS.Domain.Objects;

public class Person
{
    internal static string DELIMITER = " ";

    [JsonConstructor]
    private Person(params string[] names) 
    {
        this.Names = names;
    }
 
    public Person(string names)
    {
        this.Names = names.Split(DELIMITER);
    }

    public string[] Names { get; private set; } // TODO: test record equality

    public override string ToString()
	{
		return string.Join(DELIMITER, this.Names);
	}

    public static implicit operator string(Person member)
    {
        return member.ToString();
    }
}
