using NTS.Domain.Setup.Entities;
using NTS.Judge.Blazor.Ports;
using NTS.Judge.Hardware;

namespace NTS.Judge.Adapters.Behinds;

public class TagWriterBehind : ITagWrite
{
    private VupVD67Controller VD67Controller;

    public TagWriterBehind()
    {
        VD67Controller = new VupVD67Controller(TimeSpan.FromMilliseconds(100));
        VD67Controller.Connect();
    }

    public async Task<Tag> Write(int number)
    {
        var data = await Task.Run(VD67Controller.Read);
        var id = data.Substring(9);
        var tag = new Tag(id, number);
        VD67Controller.Write(tag.PrepareToWrite());
        return tag;
    }
}
