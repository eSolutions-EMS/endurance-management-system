using EMS.Core.Mappings;
using EMS.Core.Domain.State;
using EMS.Judge.Application.State;
using System.Reflection;

namespace EMS.Judge.Application;

public class ApplicationMappingProfile : MappingProfile
{
    public ApplicationMappingProfile()
    {
        this.CreateMap<IState, StateModel>();
    }
    
    protected override Assembly[] Assemblies => ApplicationConstants.Assemblies;
}
