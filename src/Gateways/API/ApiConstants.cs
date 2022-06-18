using EnduranceJudge.Core.Utilities;
using System.Reflection;

namespace API
{
    public class ApiConstants
    {
        public static Assembly[] Assemblies
        {
            get
            {
                var assemblies = ReflectionUtilities.GetAssemblies("API");
                return assemblies;
            }
        }
    }
}
