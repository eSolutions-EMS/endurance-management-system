using AutoMapper;
using EMS.Judge.Application.Core.Models;
using EMS.Core.Extensions;
using EMS.Core.Mappings;
using EMS.Core.Domain.Enums;
using EMS.Core.Domain.State.Competitions;

namespace EMS.Judge.Views.Content.Configuration.Children.Competitions.Configuration;

public class CompetitionViewModelMaps : ICustomMapConfiguration
{
    public void AddFromMaps(IProfileExpression profile)
    {
        profile.CreateMap<CompetitionViewModel, Competition>()
            .MapMember(x => x.Type, y => (CompetitionType)y.TypeValue);
    }

    public void AddToMaps(IProfileExpression profile)
    {
        profile.CreateMap<Competition, CompetitionViewModel>()
            .MapMember(x => x.TypeValue, y => (int)y.Type);
    }
}
