using Not.Application.Ports.CRUD;
using NTS.Domain;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Configuration;
using NTS.Domain.Enums;
using NTS.Domain.Objects;
using NTS.Judge.Blazor.Ports;
using NTS.Judge.HardwareControllers;

namespace NTS.Judge.Adapters.Behinds;

public class RfidTagReaderBehind : IRfidTagReaderBehind
{
    private VupVF747pController VF747PController;
    public RfidTagReaderBehind()
    {
        VF747PController = new VupVF747pController("192.168.68.128", TimeSpan.FromMilliseconds(10));
    }

    public async Task ReadTags()
    {
        var data_array = await Task.Run(VF747PController.StartReading);
        foreach (var tagData in data_array)
        {
            var number = int.Parse(tagData.Substring(0, 3));
            var timestamp = new Timestamp(DateTime.Now);
            var snapshot = new Snapshot(number, StaticOptions.GetRfidSnapshotType(), SnapshotMethod.RFID, timestamp);
            //process here?
        }
    }
}
