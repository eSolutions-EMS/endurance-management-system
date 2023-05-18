using Core.Utilities;
using System.Reflection;

namespace EMS.Judge.Api;

public class ApiConstants
{
    public static Assembly[] Assemblies
    {
        get
        {
            var assemblies = ReflectionUtilities.GetAssemblies("Endurance.Judge.Gateways.EMS.Judge.Api");
            return assemblies;
        }
    }
}