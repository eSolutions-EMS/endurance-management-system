using Not.Notifier;
using Not.Structures;
using NTS.Domain;
using NTS.Domain.Core.Configuration;
using NTS.Domain.Enums;
using NTS.Domain.Objects;
using NTS.Judge.Blazor.Ports;
using NTS.Judge.HardwareControllers;
using System.Linq;
using static MudBlazor.CategoryTypes;

namespace NTS.Judge.Adapters.Behinds;

public class RfidReaderBehind : IRfidReaderBehind
{
    private readonly ISnapshotProcessor _snapshotProcessor;
    private VupVF747pController VF747PController;
    private List<Snapshot> processedSnapshots = new List<Snapshot>();
    private Task _readTask;
    public RfidReaderBehind(ISnapshotProcessor snapshotProcessor)
    {
        VF747PController = new VupVF747pController("192.168.68.128", TimeSpan.FromMilliseconds(10));
        _snapshotProcessor = snapshotProcessor;
    }

    public bool IsConnected()
    {
        return VF747PController.IsConnected;
    }

    public bool IsReading()
    {
        return VF747PController.IsReading;
    }

    public void StartReading()
    {
        _readTask = Task.Run(VF747PController.StartReading);
        VF747PController.OnRead += ProcessDetectedTags;
    }

    public void StopReading()
    {
        //_readTask..Dispose();
        VF747PController.OnRead -= ProcessDetectedTags;
    }

    public async void ProcessDetectedTags(object? sender, (DateTime timestamp, string data) e)
    {
        var number = int.Parse(e.data.Substring(0, 3));
        var timestamp = new Timestamp(e.timestamp);
        var snapshot = processedSnapshots.Find(s => s.Number == number);
        if(snapshot == null)
        {   
            snapshot = new Snapshot(number, StaticOptions.GetRfidSnapshotType(), SnapshotMethod.RFID, timestamp);
            processedSnapshots.Add(snapshot);
            Process(snapshot);
        }
        else
        {
            var now = DateTime.Now;
            if ((now - snapshot!.Timestamp.DateTime) >= TimeSpan.FromSeconds(30))
            {
                snapshot.Timestamp = timestamp;
                Process(snapshot);
            }
        }

    }

    async void Process(Snapshot snapshot)
    {
        await _snapshotProcessor.Process(snapshot);
        NotifyHelper.Inform("Processed: " + snapshot.ToString());
    }
}
