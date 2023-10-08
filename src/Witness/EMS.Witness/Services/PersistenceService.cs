using EMS.Judge.Application.Common.Services;

namespace EMS.Witness.Services;

public class PersistenceService : IPersistenceService
{
    private readonly WitnessState state;
    private readonly IJsonSerializationService jsonSerializer;
    private readonly string path;

    public PersistenceService(WitnessState state, IJsonSerializationService jsonSerializer)
    {
        this.state = state;
        this.jsonSerializer = jsonSerializer;
        this.path = Path.Combine(FileSystem.Current.CacheDirectory, "e.witness");
    }

    public async Task Restore()
    {
        if (!File.Exists(path))
        {
            return;
        }
        var contents = await File.ReadAllTextAsync(path);
        var state = this.jsonSerializer.Deserialize<WitnessState>(contents);
        this.state.Set(state);
    }

    public async Task Store()
    {
        var serialized = this.jsonSerializer.Serialize(this.state);
        await File.WriteAllTextAsync(this.path, serialized);
    }
}

public interface IPersistenceService
{
    public Task Store();
    public Task Restore();
}
