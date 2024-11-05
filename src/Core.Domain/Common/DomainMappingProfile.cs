using System;
using System.Linq;
using System.Reflection;
using Core.Domain.Common.Models;
using Core.Mappings;
using Core.Utilities;

namespace Core.Domain.Common;

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
