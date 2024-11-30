using Not.Domain.Ports;
using Not.Storage.Stores.Files;
using Not.Storage.Stores.StaticOptions.Ports;

namespace Not.Storage.Stores.StaticOptions.Adapters;

public class JsonStaticOptionsProvider<T> : JsonFileStore<T>, IStaticOptionsProvider<T>
    where T : class, new()
{
    public JsonStaticOptionsProvider(IStaticOptionsConfiguration configuration)
        : base(Path.Combine(configuration.Path!, "static-options.json")) { }

    public T Get()
    {
        return Deserialize();
    }
}
