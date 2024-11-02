using System.Reflection;
using Core.Utilities;

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
