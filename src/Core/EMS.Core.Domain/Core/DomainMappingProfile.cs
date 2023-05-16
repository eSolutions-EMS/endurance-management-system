using EMS.Core.Domain.Core.Models;
using System.Reflection;
using EMS.Core.Mappings;
using EMS.Core.Utilities;
using System;
using System.Linq;

namespace EMS.Core.Domain.Core;

public class DomainMappingProfile : MappingProfile
{
    private static readonly Type DomainObjectType = typeof(IDomain);

    public DomainMappingProfile()
    {
        this.CreateDomainMaps();
    }

    protected override Assembly[] Assemblies => DomainConstants.Assemblies;

    private void CreateDomainMaps()
    {
        var types = ReflectionUtilities
            .GetInstanceTypes(this.Assemblies)
            .Where(t => DomainObjectType.IsAssignableFrom(t));
        foreach (var type in types)
        {
            this.CreateMap(type, type);
        }
    }
}
