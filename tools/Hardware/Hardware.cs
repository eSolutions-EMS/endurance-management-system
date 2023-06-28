using EMS.Judge.Application.Hardware;
using System.ComponentModel.DataAnnotations;

namespace EMS.Tools.Hardware;

public static class Hardware
{
    public static async Task Run()
    {
        //var a = new Vup.reader.VD(0);
        //var c = a.Connect();
        //var b = a.Read6C(Vup.reader.memory_bank.memory_bank_epc, 0, 0, Array.Empty<byte>(), Array.Empty<byte>());
        var wrapper = new Wrapper();
        await wrapper.WriteData("left", "13");
        await foreach (var tag in wrapper.Controller.StartReading())
        {
            Console.WriteLine(tag);
        }
    }
}

public class Wrapper
{
    public VupVD67Controller Controller { get; set; }

    public async Task WriteData(string position, string number)
    {
        number = number.ToString().PadLeft(3, '0');
        position = position.ToString().PadLeft(6, '0');

        this.Controller = new VupVD67Controller(TimeSpan.FromSeconds(1));
        this.Controller.Connect();
        var tagId = (await this.Controller.Read()).Substring(9);
        var newValue = number + position + tagId;
        
        await this.Controller.Write(newValue);
    }
}

