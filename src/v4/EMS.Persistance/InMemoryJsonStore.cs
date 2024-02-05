using Newtonsoft.Json;
using JsonNet.PrivatePropertySetterResolver;
using Common.Conventions;

namespace EMS.Persistence;

public class InMemoryJsonStore : IStore
{
    private IState? _state;
    private string? _json;

    public IState GetContext()
    {
        if (_state != null)
        {
            return _state;
        }
        var state = new State();
        if (_json == null)
        {
            return AttachContext(state);
        }
        state = JsonConvert.DeserializeObject<State>(_json, GetSettings()) ?? state;
        return AttachContext(state);
    }

    private State AttachContext(State state)
    {
        state.DetachAction = CommitToMemoryString;
        _state = state;
        return state;
    }

    private void CommitToMemoryString()
    {
        _json = JsonConvert.SerializeObject(_state, GetSettings());
        _state = null;
    }

    private JsonSerializerSettings GetSettings()
    {
        var settings = new JsonSerializerSettings();
        settings.ContractResolver = new PrivatePropertySetterResolver();
        settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
        settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
        return settings;
    }
}

public interface IStore : ISingletonService
{
    IState GetContext();
}
