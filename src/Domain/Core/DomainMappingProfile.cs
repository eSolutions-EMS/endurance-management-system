using System.Reflection;
using EnduranceJudge.Core.Mappings;

namespace EnduranceJudge.Domain.Core
{
    public class DomainMappingProfile : MappingProfile
    {
        protected override Assembly[] Assemblies => DomainConstants.Assemblies;
    }
}
