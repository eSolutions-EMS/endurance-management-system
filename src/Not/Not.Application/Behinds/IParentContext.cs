using Not.Injection;
using Not.Structures;

namespace Not.Application.Behinds;

public interface IParentContext<T> : ISingleton
    where T : IIdentifiable
{
    bool HasLoaded();
    Task Load(int parentId);
    Task Persist();
    ObservableList<T> Children { get; }
    void Add(T child);
    void Update(T child);
    void Remove(T child);
}
