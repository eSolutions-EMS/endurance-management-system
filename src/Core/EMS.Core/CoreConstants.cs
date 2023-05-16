using EMS.Core.Utilities;
using System.Reflection;

namespace EMS.Core;

public static class CoreConstants
{
    public static Assembly[] Assemblies
    {
        get
        {
            var assemblies = ReflectionUtilities.GetAssemblies("EMS.Core");
            return assemblies;
        }
    }
}
