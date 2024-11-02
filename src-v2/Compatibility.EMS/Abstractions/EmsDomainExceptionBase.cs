using System;

namespace NTS.Compatibility.EMS.Abstractions;

public abstract class EmsDomainExceptionBase : Exception
{
    protected abstract string Entity { get; }
    protected string InitMessage { get; init; }
    public override string Message => this.Prefix(this.InitMessage ?? this.Message);

    private string Prefix(string message) => $"{this.Entity} {message}";

    internal static T Create<T>(string message)
        where T : EmsDomainExceptionBase, new()
    {
        var exception = new T { InitMessage = message };
        return exception;
    }

    internal static T Create<T>(string message, params object[] arguments)
        where T : EmsDomainExceptionBase, new()
    {
        var exception = new T { InitMessage = string.Format(message, arguments) };
        return exception;
    }
}
