using EnduranceJudge.Core.Mappings;
using System.Reflection;

namespace EnduranceJudge.Gateways.Persistence.Core
{
    public class PersistenceMappingProfile : MappingProfile
    {
        protected override Assembly[] Assemblies => PersistenceConstants.Assemblies;
    }
}
