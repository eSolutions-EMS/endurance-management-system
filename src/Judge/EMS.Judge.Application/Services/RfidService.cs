using Core.ConventionalServices;
using Core.Domain.State.Participants;
using EMS.Judge.Application.Hardware;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Judge.Application.Services;
public class RfidService : IRfidService
{
    private readonly ISettings settings;
    private VupVD67Controller vd67Controller;
    private VupVF747pController vupVF747PController;
    private bool isReading;

    public RfidService(ISettings settings)
    {
        this.vd67Controller = new VupVD67Controller(TimeSpan.FromMilliseconds(100));
        this.vupVF747PController = new VupVF747pController("192.168.68.128");
        this.settings = settings;
    }

    public void ConnectReader()
    {
        if (this.settings.UseVD67InManager)
        {
            this.vd67Controller.Connect();
        }
        else
        {
            this.vupVF747PController.Connect();
        }
    }

    public void DisconnectReader()
    {
        if (this.settings.UseVD67InManager)
        {
            this.vd67Controller.Disconnect();
        }
        else
        {
            this.vupVF747PController.Disconnect();
        }
    }

    public async Task<RfidTag> Read()
    {
        this.isReading = true;
        var data = await this.vd67Controller.Read();
        this.isReading = false;
        var tag = new RfidTag(data);
        return tag;
    }

    public async IAsyncEnumerable<RfidTag> StartReading()
    {
        if (this.isReading)
        {
            yield break;
        }
        this.isReading = true;
        var tags = this.settings.UseVD67InManager
            ? this.vd67Controller.StartReading()
            : this.vupVF747PController.StartReading();
        await foreach (var data in tags)
        {
            var tag = new RfidTag(data);
            yield return tag;
        }
    }

    public void StopReading()
    {
        if (this.settings.UseVD67InManager)
        {
            this.vd67Controller.StopReading();
        }
        else
        {
            this.vupVF747PController?.StopReading();
        }
        this.isReading = false;
    }

    public async Task<RfidTag> Write(string position, string number)
    {
        var tag = await this.Read();
        tag.Position = position;
        tag.ParticipantNumber = number;
        var result = await this.vd67Controller.Write(tag.ToString());
        return tag;
    }
}

public interface IRfidService : ISingletonService
{
    Task<RfidTag> Write(string position, string number);
    Task<RfidTag> Read();
    IAsyncEnumerable<RfidTag> StartReading();
    void StopReading();
    void ConnectReader();
    void DisconnectReader();
}
