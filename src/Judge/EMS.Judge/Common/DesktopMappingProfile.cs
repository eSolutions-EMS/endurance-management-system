using Core.Mappings;
using System.Reflection;

namespace EMS.Judge.Common;

public class DesktopMappingProfile : MappingProfile
{
    protected override Assembly[] Assemblies => DesktopConstants.Assemblies;
}
