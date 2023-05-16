﻿using EMS.Core.Application.State;
using EMS.Core.Mappings;
using EMS.Core.Domain.State;
using System.Reflection;

namespace EMS.Core.Application;

public class ApplicationMappingProfile : MappingProfile
{
    public ApplicationMappingProfile()
    {
        this.CreateMap<IState, StateModel>();
    }
    
    protected override Assembly[] Assemblies => ApplicationConstants.Assemblies;
}