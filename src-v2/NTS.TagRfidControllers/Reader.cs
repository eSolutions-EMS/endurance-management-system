using NTS.TagRfidControllers.Entities;
using NTS.TagRfidControllers.HardwareControllers;

namespace NTS.TagRfidControllers.Reader;

public class Reader
{
    private VupVF747pController VF747PController;
    private ILogger logger = new Logger();
    public Reader()
    {
        VF747PController = new VupVF747pController("192.168.68.128", logger, TimeSpan.FromMilliseconds(10));
    }

    public async Task ReadTags()
    {  
        var data_array = await Task.Run(VF747PController.StartReading);
        foreach (var tagData in data_array)
        {
            var number = int.Parse(tagData.Substring(0, 3));
            Console.WriteLine("Tag Detected| " + number + " | " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
