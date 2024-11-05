using Not.Domain.Ports;

namespace Not.Storage.StaticOptions;

public class JsonStaticOptionsStore<T> : JsonStore<T>, IStaticOptionsProvider<T>
    where T : class, new()
{
    public JsonStaticOptionsStore(IStaticOptionsConfiguration configuration)
        : base(Path.Combine(configuration.Path, "static-options.json")) { }

    public T Get()
    {
        return Deserialize();
    }
}
