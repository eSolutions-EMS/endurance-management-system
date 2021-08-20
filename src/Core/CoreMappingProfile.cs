using System.Reflection;
using EnduranceJudge.Core.Mappings;

namespace EnduranceJudge.Core
{
    public class CoreMappingProfile : MappingProfile
    {
        protected override Assembly[] Assemblies => CoreConstants.Assemblies;
    }
}
