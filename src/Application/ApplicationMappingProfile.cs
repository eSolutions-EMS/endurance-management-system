using System.Reflection;
using EnduranceJudge.Core.Mappings;

namespace EnduranceJudge.Application.Core
{
    public class ApplicationMappingProfile : MappingProfile
    {
        protected override Assembly[] Assemblies => ApplicationConstants.Assemblies;
    }
}
