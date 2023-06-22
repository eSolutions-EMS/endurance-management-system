using EMS.Judge.Application.Hardware;

namespace EMS.Tools.Hardware;
public class TagIdWriter
{
    public static async Task Run()
    {
        var controller = new VupVD67Controller(TimeSpan.FromSeconds(3));
        controller.Connect();

        for (var i = 13; i <= 500; i++)
        {
            var value = i.ToString().PadLeft(12, '0');
            await controller.Write(value);
        }
    }
}
