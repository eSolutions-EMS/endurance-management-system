using Core.Mappings;
using System.Reflection;

namespace Core;

public class CoreMappingProfile : MappingProfile
{
    protected override Assembly[] Assemblies => CoreConstants.Assemblies;
}
