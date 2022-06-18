using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.State;
using System.Reflection;

namespace EnduranceJudge.Application;

public class ApplicationMappingProfile : MappingProfile
{
    public ApplicationMappingProfile()
    {
        this.CreateMap<IState, State>();
    }
    
    protected override Assembly[] Assemblies => ApplicationConstants.Assemblies;
}
