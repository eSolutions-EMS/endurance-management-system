using Not.Domain.Ports;

namespace Not.Storage.StaticOptions;

public class JsonStaticOptionsStore<T> : JsonStore<T>, IStaticOptionsProvider<T>
    where T : class, new()
{
    public JsonStaticOptionsStore(IStaticOptionsConfiguration configuration) : base(configuration.Path)
    {
    }

    public T Get()
    {
        return Deserialize();
    }
}
