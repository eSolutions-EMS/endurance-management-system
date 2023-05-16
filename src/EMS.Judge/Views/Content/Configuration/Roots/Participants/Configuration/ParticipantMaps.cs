using AutoMapper;
using EMS.Judge.Core.Components.Templates.SimpleListItem;
using EMS.Core.Application.Core.Models;
using EMS.Core.Mappings;
using EMS.Core.Domain.State.Athletes;
using EMS.Core.Domain.State.Horses;
using EMS.Core.Domain.State.Participants;

namespace EMS.Judge.Views.Content.Configuration.Roots.Participants.Configuration;

public class ParticipantMaps : ICustomMapConfiguration
{
    public void AddFromMaps(IProfileExpression profile)
    {
        profile.CreateMap<Participant, ListItemModel>();
        profile.CreateMap<Athlete, SimpleListItemViewModel>();
        profile.CreateMap<Horse, SimpleListItemViewModel>();
    }

    public void AddToMaps(IProfileExpression profile)
    {
    }
}
