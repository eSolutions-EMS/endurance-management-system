using AutoMapper;
using EMS.Judge.Application.Common.Models;
using Core.Extensions;
using Core.Mappings;
using Core.Domain.Enums;
using Core.Domain.State.Competitions;

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
