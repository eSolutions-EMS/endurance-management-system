using NTS.Domain.Setup.Entities;
using NTS.Judge.Blazor.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Judge.Adapters.Behinds;
public class TagBehind : ITagBehind
{
    public async Task<Tag> WriteTag(int number)
    {
        await Task.Delay(2000);
        return new Tag("33", number);
    }
}
