using EMS.Core.Mappings;
using System.Reflection;

namespace EMS.Judge.Core;

public class DesktopMappingProfile : MappingProfile
{
    protected override Assembly[] Assemblies => DesktopConstants.Assemblies;
}
