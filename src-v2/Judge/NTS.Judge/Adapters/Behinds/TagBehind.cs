using NTS.Domain.Setup.Entities;
using NTS.Judge.Blazor.Ports;

namespace NTS.Judge.Adapters.Behinds;

public class TagBehind : ITagBehind
{
    public async Task<Tag> WriteTag(int number)
    {
        await Task.Delay(2000);
        return new Tag("33", number);
    }
}
