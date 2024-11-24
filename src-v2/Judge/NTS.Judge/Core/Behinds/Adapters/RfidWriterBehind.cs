using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Domain.Setup.Entities;
using NTS.Judge.ACL.RFID;
using NTS.Judge.Blazor.Setup.Combinations.RFID;

namespace NTS.Judge.Core.Behinds.Adapters;

public class RfidWriterBehind : IRfidWriterBehind
{
    VupVD67Controller _vD67Controller;

    public RfidWriterBehind()
    {
        _vD67Controller = new VupVD67Controller(TimeSpan.FromMilliseconds(100));
    }

    public async Task<Tag> WriteTag(int number)
    {
        Tag function()
        {
            var data = _vD67Controller.Read();
            var id = data.Substring(9);
            var tag = new Tag(id, number);
            var writeData = tag.PrepareToWrite();
            _vD67Controller.Write(writeData);
            return tag;
        }
        var tag = await Task.Run(function);
        return tag;
    }
}
