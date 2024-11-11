using System.Reflection;
using Core.Mappings;

namespace EMS.Judge.Common;

public class DesktopMappingProfile : MappingProfile
{
    protected override Assembly[] Assemblies => DesktopConstants.Assemblies;
}
