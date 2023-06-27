using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vup.reader;

namespace EMS.Judge.Application.Hardware;

public class VupVF747pController : RfidController
{ 
    private const int MAX_POWER = 27;
    private readonly NetVupReader reader;
    private readonly string ipAddress;
    private readonly List<long> slowReadTimeMs = new();

    protected override string Device => "VF747p";

    public VupVF747pController(string ipAddress, TimeSpan? throttle = null) : base(throttle)
    {
        this.ipAddress = ipAddress;
        this.reader = new NetVupReader(ipAddress, 1969, transport_protocol.tcp);
    }

    public override void Connect()
    {
        var connectionResult = this.reader.Connect();
        if (!connectionResult.Success)
        {
            var message = string.IsNullOrEmpty(connectionResult.Message)
                ? $"Unable to connect to VUP reader on IP '{this.ipAddress}'." +
                    $" Make sure it's connected to the network and use 'Reconnect Hardware'"
                : connectionResult.Message;
            this.RaiseMessage(message);
            return;
        }
        this.IsConnected = true;
        this.SetPower(MAX_POWER);
        this.RaiseMessage($"Connected!");
        Console.WriteLine("==========================================================================================");
        Console.WriteLine($"= {this.Device} connection established");
        Console.WriteLine("==========================================================================================");
    }

    public async IAsyncEnumerable<string> StartReading()
    {
        if (!this.IsConnected)
        {
            this.Connect();
        }
        var antennaIndices = this.GetAntennaIndices();
        this.IsReading = true;
        while (this.IsReading)
        {
            foreach (var tag in this.ReadTags(antennaIndices))
            {
                yield return tag;
            }
            await Task.Delay(this.throttle);
        }
    }

    public void StopReading()
    {
        this.IsReading = false;
    }

    public override void Disconnect()
    {
        if (this.IsConnected)
        {
            this.reader.Disconnect();
            this.IsConnected = false;
            this.RaiseMessage("Disconnected!");
        }
    }

    public void SetPower(int power)
    {
        var antennaIndices = this.GetAntennaIndices();
        foreach (var i in antennaIndices)
        {
            var setPowerResult = this.reader.SetAntPower(i, power);
            if (!setPowerResult.Success)
            {
                var message = $"Error while setting antenna power: {setPowerResult.Message}";
                this.RaiseMessage(message);
            }
        }
    }

    private int[] GetAntennaIndices()
    {
        var antennaRead = this.reader.GetAntCount();
        if (!antennaRead.Success)
        {
            var message = $"Error in antenna read: {antennaRead.Message}";
            this.RaiseMessage(message);
            return Array.Empty<int>();
        }
        var indices = new List<int>();
        for (var i = 1; i <= antennaRead.Result; i++)
        {
            indices.Add(i);
        }
        return indices.ToArray();
    }

    private DateTime? disconnectedMessageTime;
    private IEnumerable<string> ReadTags(IEnumerable<int> antennaIndices)
    {
        var timer = new Stopwatch();
        foreach (var i in antennaIndices)
        {
            this.reader.SetWorkAnt(i);
            timer.Start();

            var tagsBytes = this.reader.List6C(memory_bank.memory_bank_epc, 0, 12 ,Convert.FromHexString("00000000"));

            timer.Stop();
            if (this.disconnectedMessageTime == null
                || DateTime.Now - this.disconnectedMessageTime > TimeSpan.FromMinutes(1))
            {
                if (timer.ElapsedMilliseconds is > 1000 or < 5)
                {
                    this.slowReadTimeMs.Add(timer.ElapsedMilliseconds);
                }
                if (this.slowReadTimeMs.Count >= 10)
                {
                    this.slowReadTimeMs.Clear();
                    this.disconnectedMessageTime = DateTime.Now;
                    this.RaiseMessage("RFID tag read responses indicate the Vup " +
                        "Reader might be disconnected. Look for VUP console logs.");
                }
            }

            if (tagsBytes.Success)
            {
                var tags2 = tagsBytes.Result.Select(x => Encoding.ASCII.GetString(x.Id)).FirstOrDefault();
                var tags3 = tagsBytes.Result.Select(x => Encoding.UTF32.GetString(x.Id)).FirstOrDefault();
                var tags4 = tagsBytes.Result.Select(x => Encoding.Unicode.GetString(x.Id)).FirstOrDefault();
                var tags5 = tagsBytes.Result.Select(x => Encoding.Latin1.GetString(x.Id)).FirstOrDefault();
                var tags6 = tagsBytes.Result.Select(x => Convert.ToHexString(x.Id)).FirstOrDefault();
                return tagsBytes.Result.Select(x => Encoding.UTF8.GetString(x.Data));
            }
        }
        return Enumerable.Empty<string>();
    }
}
