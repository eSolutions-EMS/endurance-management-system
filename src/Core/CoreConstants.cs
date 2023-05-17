using Core.Utilities;
using System.Reflection;

namespace Core;

public static class CoreConstants
{
    public static Assembly[] Assemblies
    {
        get
        {
            var assemblies = ReflectionUtilities.GetAssemblies("Core");
            return assemblies;
        }
    }
}
