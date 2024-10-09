using AngleSharp.Dom;
using Not.Domain;
using Not.Events;
using Not.Injection;

namespace Not.Blazor.Ports.Behinds;

public interface IBehindContext<T> : IBehindContext, ISingletonService
    where T : DomainEntity
{
    T? Entity { get; }
}

public interface IBehindContext
{
    EventManager Loaded { get; }
    bool HasLoaded();
    Task Update();
}