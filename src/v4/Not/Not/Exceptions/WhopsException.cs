namespace Common.Exceptions;

/// <summary>
/// This exception is intended to be SEEN BY DEVS ONLY.
/// TODO: links to documentation and useful reads.
/// </summary>
public class WhopsException : Exception
{
    public WhopsException(string message) : base($"WHOOPS! It seems you messed up: {message}")
    {
    }
}
