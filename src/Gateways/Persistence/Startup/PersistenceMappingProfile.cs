using EnduranceJudge.Application;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Application.Models;
using System.Reflection;

namespace EnduranceJudge.Gateways.Persistence.Startup;

public class PersistenceMappingProfile : MappingProfile
{
    public PersistenceMappingProfile()
    {
        this.CreateMap<IState, State>();
    }

    protected override Assembly[] Assemblies { get; } = PersistenceConstants.Assemblies;
}
