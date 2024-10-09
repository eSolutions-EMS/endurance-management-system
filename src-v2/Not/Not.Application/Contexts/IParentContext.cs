using Not.Blazor.Ports.Behinds;
using Not.Structures;

namespace Not.Application.Contexts;

public interface IParentContext<T> : IBehindContext
    where T : IIdentifiable
{
    Task Load(int parentId);
    EntitySet<T> Children { get; set; }
    void Add(T child);
    void Update(T child);
    void Remove(T child);
}
