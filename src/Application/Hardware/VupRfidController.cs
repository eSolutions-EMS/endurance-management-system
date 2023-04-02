using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vup.reader;

namespace EnduranceJudge.Application.Hardware;

public class VupRfidController
{
    private const int MAX_POWER = 27;
    private readonly NetVupReader reader;
    
    public VupRfidController(string ipAddress)
    {
        this.reader = new NetVupReader(ipAddress, 1969, transport_protocol.tcp);
    }

    public event EventHandler<IEnumerable<string>> Read;
    public void RaiseRead(IEnumerable<string> tagId)
    {
        this.Read!.Invoke(this, tagId);
    }
    public event EventHandler<string> Error;
    public void RaiseError(string message)
    {
        this.Error!.Invoke(this, message);
    }
    public bool IsPolling { get; private set; }

    public void Connect()
    {
        var connectionResult = this.reader.Connect();
        if (!connectionResult.Success)
        {
            this.RaiseError(connectionResult.Message ?? "Unknown failure. Please try again");
        }
        this.SetPower(MAX_POWER);
    }

    public async Task StartPolling()
    {
        var antennaIndices = this.GetAntennaIndices();
        this.IsPolling = true;

        while (this.IsPolling)
        {
            foreach (var i in antennaIndices)
            {
                this.reader.SetWorkAnt(i);
                var tagRead = this.reader.List6C(
                    memory_bank.memory_bank_epc,
                    0,
                    12,
                    Convert.FromHexString("00000000"));
                if (!tagRead.Success)
                {
                    var message = $"Error in tags read: {tagRead.Message}";
                    this.RaiseError(message);
                    return;
                }

                var tags = tagRead.Result.Select(x => Convert.ToHexString(x.Id));
                this.RaiseRead(tags);
            }
            await Task.Delay(TimeSpan.FromMilliseconds(1));
        }
    }

    public void StopPolling()
    {
        this.IsPolling = false;
    }

    public void SetPower(int power)
    {
        var antennaIndices = this.GetAntennaIndices();
        foreach (var i in antennaIndices)
        {
            var setPowerResult = this.reader.SetAntPower(i, MAX_POWER);
            if (!setPowerResult.Success)
            {
                var message = $"Error while setting antenna power: {setPowerResult.Message}";
                this.RaiseError(message);
            }
        }
    }

    private int[] GetAntennaIndices()
    {
        var antennaRead = this.reader.GetAntCount();
        if (!antennaRead.Success)
        {
            var message = $"Error in antenna read: {antennaRead.Message}";
            this.RaiseError(message);
            return Array.Empty<int>();
        }
        var indices = new List<int>();
        for (var i = 1; i <= antennaRead.Result; i++)
        {
            indices.Add(i);
        }
        return indices.ToArray();
    }
}
