using AutoMapper;
using Core.Mappings;
using Personnelz = Core.Domain.State.Personnels.Personnel;

namespace EMS.Judge.Views.Content.Configuration.Children.Personnel.Configuration;

public class PersonnelMaps : ICustomMapConfiguration
{
    public void AddFromMaps(IProfileExpression profile)
    {
        profile
            .CreateMap<Personnelz, PersonnelViewModel>()
            .ForMember(x => x.Role, opt => opt.Ignore())
            .ForMember(x => x.RoleId, opt => opt.MapFrom(y => (int)y.Role));
    }

    public void AddToMaps(IProfileExpression profile) { }
}
