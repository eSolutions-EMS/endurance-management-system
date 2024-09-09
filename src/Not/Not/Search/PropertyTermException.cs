using Not.Exceptions;

namespace Not.Search;

public class PropertyTermException : NotException
{
    public PropertyTermException(string message) : base(message)
    {
    }
}
