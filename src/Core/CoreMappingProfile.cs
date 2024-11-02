using System.Reflection;
using Core.Mappings;

namespace Core;

public class CoreMappingProfile : MappingProfile
{
    protected override Assembly[] Assemblies => CoreConstants.Assemblies;
}
