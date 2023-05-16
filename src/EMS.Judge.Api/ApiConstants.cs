using EnduranceJudge.Core.Utilities;
using System.Reflection;

namespace Endurance.Judge.Gateways.API
{
    public class ApiConstants
    {
        public static Assembly[] Assemblies
        {
            get
            {
                var assemblies = ReflectionUtilities.GetAssemblies("Endurance.Judge.Gateways.API");
                return assemblies;
            }
        }
    }
}
