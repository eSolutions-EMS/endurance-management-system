using System;
using System.Threading.Tasks;

namespace EMS.Judge.Application.Hardware;

public class VD67Controller
{
    public async Task<string> Write(string number)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return "some-tag-id";
    }
}
