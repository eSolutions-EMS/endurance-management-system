using System.Reflection;
using Core.Utilities;

namespace EMS.Judge.Api;

public class ApiConstants
{
    public static Assembly[] Assemblies
    {
        get
        {
            var assemblies = ReflectionUtilities.GetAssemblies("EMS.Judge.Api");
            return assemblies;
        }
    }
}
