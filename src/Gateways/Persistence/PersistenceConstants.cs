using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.State.Countries;
using System.Collections.Generic;
using System.Reflection;

namespace EnduranceJudge.Gateways.Persistence;

public static class PersistenceConstants
{
    public const string BackupFileName = "backup.txt";

    public static Assembly[] Assemblies
    {
        get
        {
            var assemblies = ReflectionUtilities.GetAssemblies("EnduranceJudge.Gateways.Persistence");
            return assemblies;
        }
    }
}
