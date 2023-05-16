using AutoMapper;
using EMS.Core.Mappings;

namespace EMS.Judge.Views.Content.Configuration.Children.Personnel.Configuration;

public class PersonnelMaps : ICustomMapConfiguration
{
    public void AddFromMaps(IProfileExpression profile)
    {
        profile.CreateMap<EMS.Core.Domain.State.Personnels.Personnel, PersonnelViewModel>()
            .ForMember(x => x.Role, opt => opt.Ignore())
            .ForMember(x => x.RoleId, opt => opt.MapFrom(y => (int)y.Role));
    }

    public void AddToMaps(IProfileExpression profile)
    {
    }
}
