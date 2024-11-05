using System.Reflection;
using Core.Utilities;

namespace EMS.Witness;

public static class WitnessConstants
{
    public static Assembly[] Assemblies => ReflectionUtilities.GetAssemblies("EMS.Witness");
}
