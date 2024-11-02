using AutoMapper;
using Core.Domain.State.Athletes;
using Core.Domain.State.Horses;
using Core.Domain.State.Participants;
using Core.Mappings;
using EMS.Judge.Application.Common.Models;
using EMS.Judge.Common.Components.Templates.SimpleListItem;

namespace EMS.Judge.Views.Content.Configuration.Roots.Participants.Configuration;

public class ParticipantMaps : ICustomMapConfiguration
{
    public void AddFromMaps(IProfileExpression profile)
    {
        profile.CreateMap<Participant, ListItemModel>();
        profile.CreateMap<Athlete, SimpleListItemViewModel>();
        profile.CreateMap<Horse, SimpleListItemViewModel>();
    }

    public void AddToMaps(IProfileExpression profile) { }
}
