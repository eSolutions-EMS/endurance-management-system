using NTS.TagRfidControllers.HardwareControllers;

namespace NTS.TagRfidControllers.Entities;
public class Logger : ILogger
{
    public void Log(string name, string error)
    {
        Console.WriteLine(name, error);
    }
}
