using AutoMapper;
using EMS.Judge.Application.Core.Models;
using Core.Mappings;
using Core.Domain.State.Participants;

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
