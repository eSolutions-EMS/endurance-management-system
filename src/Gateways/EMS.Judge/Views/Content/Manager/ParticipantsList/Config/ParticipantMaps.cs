using AutoMapper;
using EnduranceJudge.Application.Core.Models;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.State.Participants;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager.ParticipantsList.Config;

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
