using NTS.Domain;
using NTS.Domain.Core.Configuration;
using NTS.Domain.Objects;
using NTS.Domain.Setup.Entities;
using NTS.Judge.Blazor.Ports;
using NTS.Judge.Hardware;

namespace NTS.Judge.Adapters.Behinds;

public class TagReaderBehind : ITagRead
{
    private VupVF747pController VF747PController;

    public TagReaderBehind()
    {
        VF747PController = new VupVF747pController("192.168.68.128", TimeSpan.FromMilliseconds(10));
        VF747PController.Connect();
    }

    public async Task<IEnumerable<RfidSnapshot>> ReadTags()
    {
        var tags = new List<RfidSnapshot>();
        var data_array = await Task.Run(VF747PController.StartReading);
        foreach(var tagData in data_array)
        {
            var number = int.Parse(tagData.Substring(0, 3));
            tags.Add(new RfidSnapshot(number, StaticOptions.RfidSnapshotType(), new Timestamp(DateTime.Now)));
        }
        return tags;
    }
}
