using System.Reflection;
using Core.Domain.State;
using Core.Mappings;
using EMS.Judge.Application.State;

namespace EMS.Judge.Application;

public class ApplicationMappingProfile : MappingProfile
{
    public ApplicationMappingProfile()
    {
        this.CreateMap<IState, StateModel>();
    }

    protected override Assembly[] Assemblies => ApplicationConstants.Assemblies;
}
