using EMS.Judge.Application.Hardware;

namespace EMS.Tools.Hardware;
public class TagIdWriter
{
    public static async Task Run()
    {
        var controller = new VupVD67Controller(TimeSpan.FromSeconds(3));
        controller.Connect();
        string? command = null;
        do
        {
            Console.WriteLine("Scan tag");
            var tag = await controller.Read();
            Console.WriteLine($"Tag: {tag}");
            var id = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(id))
            {
                id = Console.ReadLine();
            }
            if (id == "next")
            {
                continue;
            }
            var prev = tag[..9];
            var value = id.PadLeft(3, '0');
            var result = await controller.Write(prev + value);
            Console.WriteLine($"Result: {result}");
            Console.WriteLine("Next:");
            command = Console.ReadLine();
        } while (command != "stop");
    }
}
