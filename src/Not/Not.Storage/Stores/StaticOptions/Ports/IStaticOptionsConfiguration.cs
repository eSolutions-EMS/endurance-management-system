namespace Not.Storage.Stores.StaticOptions.Ports;

public interface IStaticOptionsConfiguration
{
    string? Path { get; } // TODO: probably shouldn't be nullable as they are required
}
