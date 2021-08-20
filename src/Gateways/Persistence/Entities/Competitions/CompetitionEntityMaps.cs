using AutoMapper;
using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Event.Competitions;
using System.Linq;
using ImportCompetition = EnduranceJudge.Domain.Aggregates.Import.Competitions.Competition;

namespace EnduranceJudge.Gateways.Persistence.Entities.Competitions
{
    public class CompetitionEntityMaps : ICustomMapConfiguration
    {
        public void AddFromMaps(IProfileExpression profile)
        {
            profile.CreateMap<Competition, CompetitionEntity>()
                .ForMember(ce => ce.ParticipantsInCompetitions, opt => opt.MapFrom(c => c.Participants))
                .AfterMap((c, ce) =>
                {
                    foreach (var participantInCompetition in ce.ParticipantsInCompetitions
                        .Where(pic => pic.CompetitionId == default))
                    {
                        participantInCompetition.Competition = ce;
                        participantInCompetition.CompetitionId = ce.Id;
                    }
                });

            profile.CreateMap<ImportCompetition, CompetitionEntity>()
                .ForMember(ce => ce.ParticipantsInCompetitions, opt => opt.MapFrom(c => c.Participants))
                .AfterMap((c, ce) =>
                {
                    foreach (var participantInCompetition in ce.ParticipantsInCompetitions
                        .Where(pic => pic.CompetitionId == default))
                    {
                        participantInCompetition.Competition = ce;
                        participantInCompetition.CompetitionId = ce.Id;
                    }
                });
        }

        public void AddToMaps(IProfileExpression profile)
        {
            profile.CreateMap<CompetitionEntity, Competition>()
                .ForMember(c => c.Participants, opt => opt.MapFrom(ce => ce.ParticipantsInCompetitions));

            profile.CreateMap<CompetitionEntity, ImportCompetition>()
                .ForMember(c => c.Participants, opt => opt.MapFrom(ce => ce.ParticipantsInCompetitions));

            profile.CreateMap<CompetitionEntity, ListItemModel>()
                .ForMember(x => x.Name, opt => opt.MapFrom(y => y.Type.ToString()));

            profile.CreateMap<CompetitionEntity, CompetitionDependantModel>()
                .ForMember(
                    d => d.Participants,
                    opt => opt.MapFrom(y => y.ParticipantsInCompetitions.Select(pic => pic.Participant)));
        }
    }
}
