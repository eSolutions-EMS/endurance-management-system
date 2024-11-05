using System.Collections.Concurrent;
using System.Diagnostics;
using System.Dynamic;
using System.Text;
using Not.Logging;
using NTS.Domain;
using NTS.Domain.Core.Configuration;
using NTS.Domain.Enums;
using NTS.Domain.Objects;
using NTS.Domain.Setup.Entities;
using Vup.reader;

namespace NTS.Judge.ACL.RFID;

public class VupVF747pController : RfidController
{
    private const int MAX_POWER = 27;
    private NetVupReader reader;
    private readonly string ipAddress;

    protected override string Device => "VF747p";

    public VupVF747pController(string ipAddress, TimeSpan? throttle = null)
        : base(throttle)
    {
        this.ipAddress = ipAddress;
        reader = new NetVupReader(ipAddress, 1969, transport_protocol.tcp);
    }

    public override void Connect()
    {
        var connectionResult = reader.Connect();
        if (!connectionResult.Success)
        {
            var message = string.IsNullOrEmpty(connectionResult.Message)
                ? $"Unable to connect to VUP reader on IP '{ipAddress}'."
                    + $" Make sure it's connected to the network and use 'Reconnect Hardware'"
                : connectionResult.Message;
            RaiseError(message);
            return;
        }
        IsConnected = true;
        SetPower(MAX_POWER);
        RaiseMessage($"Connected");
    }

    public void StartReading()
    {
        if (!IsConnected)
        {
            Connect();
        }
        var antennaIndices = GetAntennaIndices();
        IsReading = true;
        while (IsReading && reader.IsConnected)
        {
            foreach (var tag in ReadTags(antennaIndices))
            {
                OnReadEvent((DateTime.Now, tag));
            }
            Thread.Sleep(throttle);
        }
    }

    public void StopReading()
    {
        IsReading = false;
    }

    public override void Disconnect()
    {
        if (IsConnected)
        {
            reader.Disconnect();
            IsConnected = false;
            RaiseMessage("Disconnected!");
        }
    }

    public void SetPower(int power)
    {
        var antennaIndices = GetAntennaIndices();
        foreach (var i in antennaIndices)
        {
            var setPowerResult = reader.SetAntPower(i, power);
            if (!setPowerResult.Success)
            {
                var message = $"Error while setting antenna power: {setPowerResult.Message}";
                RaiseError(message);
            }
        }
    }

    private int[] GetAntennaIndices()
    {
        var antennaRead = reader.GetAntCount();
        if (!antennaRead.Success)
        {
            var message = $"Error in antenna read: {antennaRead.Message}";
            RaiseError(message);
            return Array.Empty<int>();
        }
        var indices = new List<int>();
        for (var i = 1; i <= antennaRead.Result; i++)
        {
            indices.Add(i);
        }
        return indices.ToArray();
    }

    private int _oneCount = 0;
    private int _twoCount = 0;
    private int _threeCount = 0;

    private IEnumerable<string> ReadTags(IEnumerable<int> antennaIndices)
    {
        var readTimer = new Stopwatch();
        var reconnectTimer = new Stopwatch();
        foreach (var i in antennaIndices)
        {
            reader.SetWorkAnt(i);

            readTimer.Start();
            var tagsBytes = reader.List6C(
                memory_bank.memory_bank_epc,
                TAG_READ_START_INDEX,
                TAG_DATA_LENGTH,
                Convert.FromHexString("00000000")
            );
            readTimer.Stop();

            if (readTimer.ElapsedMilliseconds is < 5)
            {
                reconnectTimer.Start();
                var sb = new StringBuilder();
                sb.AppendLine(
                    $"VF747p reader response indicates a disconnect. Response time: '{readTimer.ElapsedMilliseconds}'"
                );
                RaiseError(sb.ToString());
                Reconnect();
                reconnectTimer.Stop();
                sb.AppendLine($"Reconnected after '{reconnectTimer.ElapsedMilliseconds}'");
                LoggingHelper.Information("VF747p-disconnected\n" + sb.ToString());
            }

            if (tagsBytes.Success)
            {
                if (i == 1)
                    _oneCount++;
                if (i == 2)
                    _twoCount++;
                if (i == 3)
                    _threeCount++;

                var tags = tagsBytes.Result.Select(x => ConvertToString(x.Id));
                foreach (var tag in tags)
                {
                    Console.WriteLine(
                        $"Detected: {tag} antenna: {i} one:{_oneCount}, two:{_twoCount}, three :{_threeCount}"
                    );
                }
                return tags;
            }
        }
        return Enumerable.Empty<string>();
    }

    private void Reconnect()
    {
        Disconnect();
        reader = new NetVupReader(ipAddress, 1969, transport_protocol.tcp);
        var counter = 0;
        var before = DateTime.Now;
        do
        {
            Console.WriteLine($"Attempting to reconnect: {counter}");
            Connect();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            counter++;
        } while (!IsConnected);
        var after = DateTime.Now;
        Console.WriteLine(
            $"Reconnected after '{counter}' attempts and '{(after - before).TotalSeconds}' seconds"
        );
    }
}
