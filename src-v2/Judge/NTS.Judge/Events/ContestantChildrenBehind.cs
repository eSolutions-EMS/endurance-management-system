using Not.Blazor.Ports.Behinds;
using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Events;

public class ContestantChildrenBehind 
{
    public Task Initialize(int id)
    {
        return Task.CompletedTask;
    }
}
