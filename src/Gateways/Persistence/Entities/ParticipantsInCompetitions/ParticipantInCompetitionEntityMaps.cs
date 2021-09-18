using AutoMapper;
using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Event.Participants;
using EnduranceJudge.Domain.Aggregates.Manager.ParticipationsInCompetitions;
using ImportParticipant = EnduranceJudge.Domain.Aggregates.Import.Participants.Participant;

namespace EnduranceJudge.Gateways.Persistence.Entities.ParticipantsInCompetitions
{
    public class ParticipantInCompetitionEntityMaps : ICustomMapConfiguration
    {
        public void AddFromMaps(IProfileExpression profile)
        {
            profile.CreateMap<Participant, ParticipantInCompetitionEntity>()
                .ForMember(pic => pic.Id, opt => opt.Ignore())
                .ForMember(pic => pic.Competition, opt => opt.Ignore())
                .ForMember(pic => pic.CompetitionId, opt => opt.Ignore())
                .ForMember(pic => pic.ParticipantId, opt => opt.MapFrom(p => p.Id))
                .ForMember(pic => pic.Participant, opt => opt.MapFrom(p => p));

            profile.CreateMap<ImportParticipant, ParticipantInCompetitionEntity>()
                .ForMember(pic => pic.Id, opt => opt.Ignore())
                .ForMember(pic => pic.Competition, opt => opt.Ignore())
                .ForMember(pic => pic.CompetitionId, opt => opt.Ignore())
                .ForMember(pic => pic.ParticipantId, opt => opt.MapFrom(p => p.Id))
                .ForMember(pic => pic.Participant, opt => opt.MapFrom(p => p));

            profile.CreateMap<ParticipationInCompetition, ParticipantInCompetitionEntity>()
                .ForMember(d => d.Competition, opt => opt.Ignore());
        }

        public void AddToMaps(IProfileExpression profile)
        {
            profile.CreateMap<ParticipantInCompetitionEntity, Participant>()
                .ForMember(p => p.Id, opt => opt.MapFrom(pic => pic.ParticipantId))
                .ForMember(p => p.Number, opt => opt.MapFrom(pic => pic.Participant.Number))
                .ForMember(p => p.MaxAverageSpeedInKmPh, opt => opt.MapFrom(pic => pic.Participant.MaxAverageSpeedInKmPh))
                .ForMember(p => p.RfId, opt => opt.MapFrom(pic => pic.Participant.RfId));

            profile.CreateMap<ParticipantInCompetitionEntity, ParticipationInCompetition>()
                .MapMember(d => d.Category, s => s.Participant.Athlete.Category)
                .MapMember(d => d.MaxAverageSpeedInKpH, s => s.Participant.MaxAverageSpeedInKmPh);
        }
    }
}
