using EnduranceJudge.Core.Mappings;
using System.Reflection;

namespace EnduranceJudge.Gateways.Desktop.Core
{
    public class DesktopMappingProfile : MappingProfile
    {
        protected override Assembly[] Assemblies => DesktopConstants.Assemblies;
    }
}
