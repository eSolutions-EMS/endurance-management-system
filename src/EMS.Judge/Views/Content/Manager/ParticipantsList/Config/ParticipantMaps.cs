using AutoMapper;
using EMS.Core.Application.Core.Models;
using EMS.Core.Mappings;
using EMS.Core.Domain.State.Participants;

namespace EMS.Judge.Views.Content.Manager.ParticipantsList.Config;

public class ParticipantMaps : ICustomMapConfiguration
{
    public void AddFromMaps(IProfileExpression profile)
    {
        profile.CreateMap<Participant, ListItemModel>();
    }

    public void AddToMaps(IProfileExpression profile)
    {
    }
}
