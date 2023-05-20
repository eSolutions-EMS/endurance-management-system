using Core.Utilities;
using System.Reflection;

namespace EMS.Witness;

public static class WitnessConstants
{
    public static Assembly[] Assemblies
        => ReflectionUtilities.GetAssemblies("EMS.Witness");
}
