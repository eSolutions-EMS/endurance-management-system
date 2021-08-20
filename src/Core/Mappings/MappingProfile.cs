using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using System.Collections.Generic;
using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Core.Models;

namespace EnduranceJudge.Core.Mappings
{
    public abstract class MappingProfile : Profile
    {
        private static readonly Type MyObjectType = typeof(IObject);
        private static readonly Type MapFromType = typeof(IMapFrom<>);
        private static readonly Type MapToType = typeof(IMapTo<>);
        private static readonly Type MapType = typeof(IMap<>);
        private static readonly Type CustomMapConfigurationType = typeof(ICustomMapConfiguration);

        protected MappingProfile()
        {
            this.AddConventionalMaps();
            this.AddCustomMaps();
        }

        protected abstract Assembly[] Assemblies { get; }

        protected void AddConventionalMaps()
        {
            var configurations = this
                .GetInstanceTypes()
                .Where(t => MyObjectType.IsAssignableFrom(t))
                .Select(t => new
                {
                    Type = t,
                    MapFromTypes = GetMappingModels(t, MapFromType),
                    MapToTypes = GetMappingModels(t, MapToType),
                    MapTypes = GetMappingModels(t, MapType),
                })
                .ToList();

            foreach (var configuration in configurations)
            {
                configuration.MapFromTypes.ForEach(mapFrom => this.CreateMap(mapFrom, configuration.Type));
                configuration.MapToTypes.ForEach(mapTo => this.CreateMap(configuration.Type, mapTo));
                configuration.MapTypes.ForEach(map =>
                {
                    this.CreateMap(configuration.Type, map);
                    this.CreateMap(map, configuration.Type);
                });

                this.CreateMap(configuration.Type, configuration.Type);
            }
        }

        private void AddCustomMaps()
        {
            var configurations = this
                .GetInstanceTypes()
                .Where(t => CustomMapConfigurationType.IsAssignableFrom(t))
                .Select(t => Activator.CreateInstance(t)!)
                .Cast<ICustomMapConfiguration>()
                .ToList();

            foreach (var configuration in configurations)
            {
                configuration.AddFromMaps(this);
                configuration.AddToMaps(this);
            }
        }

        private IEnumerable<Type> GetInstanceTypes()
        {
            var types = this.Assemblies
                .SelectMany(a => a.GetExportedTypes())
                .Where(t => t.IsClass && !t.IsAbstract);

            return types;
        }

        protected static IEnumerable<Type> GetMappingModels(Type source, Type mappingType)
            => source
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == mappingType)
                .Select(i => i.GetGenericArguments().First());
    }
}
