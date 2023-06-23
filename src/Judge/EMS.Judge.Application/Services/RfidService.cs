using Core.ConventionalServices;
using Core.Domain.State.Participants;
using EMS.Judge.Application.Hardware;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Judge.Application.Services;
public class RfidService : IRfidService
{
    private VupVD67Controller vd67Controller;

    public RfidService()
    {
        this.vd67Controller = new VupVD67Controller(TimeSpan.FromMilliseconds(100));
    }

    public async Task<RfidTag> Read()
    {
        var data = await this.vd67Controller.Read();
        var tag = new RfidTag(data);
        return tag;
    }

    public IAsyncEnumerable<RfidTag> StartReading()
    {
        throw new System.NotImplementedException();
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
}
