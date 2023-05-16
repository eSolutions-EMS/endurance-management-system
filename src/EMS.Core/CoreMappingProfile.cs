using EMS.Core.Mappings;
using System.Reflection;

namespace EMS.Core;

public class CoreMappingProfile : MappingProfile
{
    protected override Assembly[] Assemblies => CoreConstants.Assemblies;
}
