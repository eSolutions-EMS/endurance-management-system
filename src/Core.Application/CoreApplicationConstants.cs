using Core.Utilities;
using System.Reflection;

namespace Core.Application;

public class CoreApplicationConstants
{
    public const int NETWORK_BROADCAST_PORT = 21337;

    public static class Apps
    {
        public const string JUDGE = "Judge";
        public const string WITNESS = "Witness";
    }

    public static class Api
    {
        public const string WITNESS = "witness";
        public const string STARTLIST = "startlist";
    }

    public static Assembly[] Assemblies
        => ReflectionUtilities.GetAssemblies("Core.Application");
}
