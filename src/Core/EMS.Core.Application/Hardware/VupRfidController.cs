using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Vup.reader;

namespace EMS.Core.Application.Hardware;

public class VupRfidController
{
    private const int MAX_POWER = 27;
    private readonly NetVupReader reader;
    private readonly string ipAddress;
    private bool cannotConnect;
    private readonly List<long> slowReadTimeMs = new();

    public VupRfidController(string ipAddress)
    {
        this.ipAddress = ipAddress;
        this.reader = new NetVupReader(ipAddress, 1969, transport_protocol.tcp);
    }

    public bool IsConnected { get; private set; }
    public event EventHandler<IEnumerable<string>> ReadEvent;
    public void RaiseRead(IEnumerable<string> tagId)
    {
        this.ReadEvent!.Invoke(this, tagId);
    }
    public event EventHandler<string> MessageEvent;
    public void RaiseMessage(string message)
    {
        this.MessageEvent!.Invoke(this, message);
    }
    public bool IsPolling { get; private set; }

    public void Connect()
    {
        var connectionResult = this.reader.Connect();
        if (!connectionResult.Success)
        {
            var message = string.IsNullOrEmpty(connectionResult.Message)
                ? $"Unable to connect to VUP reader on IP '{this.ipAddress}'." +
                    $" Make sure it's connected to the network and use 'Reconnect Hardware'"
                : connectionResult.Message;
            this.RaiseMessage(message);
            this.cannotConnect = true;
            return;
        }
        this.IsConnected = true;
        this.cannotConnect = false;
        this.SetPower(MAX_POWER);
        this.RaiseMessage($"Connected!");
        Console.WriteLine("==========================================================================================");
        Console.WriteLine("= Vup Reader connection established");
        Console.WriteLine("==========================================================================================");
    }

    public async Task StartPolling()
    {
        if (this.cannotConnect)
        {
            return;
        }
        var antennaIndices = this.GetAntennaIndices();
        this.IsPolling = true;
        while (this.IsPolling)
        {
            var tags = this.ReadTags(antennaIndices);
            this.RaiseRead(tags);
            await Task.Delay(TimeSpan.FromMilliseconds(1000));
        }
    }

    public void StopPolling()
    {
        this.IsPolling = false;
    }

    public void Disconnect()
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

            var tagRead = this.reader.List6C(
                memory_bank.memory_bank_epc,
                0,
                12,
                Convert.FromHexString("00000000"));

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

            if (tagRead.Success)
            {
                return tagRead.Result.Select(x => Convert.ToHexString(x.Id));
            }
        }
        return Enumerable.Empty<string>();
    }
}
