using AutoMapper;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Event.Participants;
using ImportParticipant = EnduranceJudge.Domain.Aggregates.Import.Participants.Participant;

namespace EnduranceJudge.Gateways.Persistence.Entities.ParticipantsInCompetitions
{
    public class ParticipantInCompetitionEntityMaps : ICustomMapConfiguration
    {
        public void AddFromMaps(IProfileExpression profile)
        {
            profile.CreateMap<Participant, ParticipantInCompetition>()
                .ForMember(pic => pic.Id, opt => opt.Ignore())
                .ForMember(pic => pic.Competition, opt => opt.Ignore())
                .ForMember(pic => pic.CompetitionId, opt => opt.Ignore())
                .ForMember(pic => pic.ParticipantId, opt => opt.MapFrom(p => p.Id))
                .ForMember(pic => pic.Participant, opt => opt.MapFrom(p => p));

            profile.CreateMap<ImportParticipant, ParticipantInCompetition>()
                .ForMember(pic => pic.Id, opt => opt.Ignore())
                .ForMember(pic => pic.Competition, opt => opt.Ignore())
                .ForMember(pic => pic.CompetitionId, opt => opt.Ignore())
                .ForMember(pic => pic.ParticipantId, opt => opt.MapFrom(p => p.Id))
                .ForMember(pic => pic.Participant, opt => opt.MapFrom(p => p));
        }

        public void AddToMaps(IProfileExpression profile)
        {
            profile.CreateMap<ParticipantInCompetition, Participant>()
                .ForMember(p => p.Id, opt => opt.MapFrom(pic => pic.ParticipantId))
                .ForMember(p => p.Number, opt => opt.MapFrom(pic => pic.Participant.Number))
                .ForMember(p => p.MaxAverageSpeedInKmPh, opt => opt.MapFrom(pic => pic.Participant.MaxAverageSpeedInKmPh))
                .ForMember(p => p.RfId, opt => opt.MapFrom(pic => pic.Participant.RfId))
                .ForMember(p => p.AthleteId, opt => opt.MapFrom(pic => pic.Participant.Athlete.Id))
                .ForMember(p => p.HorseId, opt => opt.MapFrom(pic => pic.Participant.Horse.Id));

            profile.CreateMap<ParticipantInCompetition, ImportParticipant>()
                .ForMember(p => p.Id, opt => opt.MapFrom(pic => pic.ParticipantId))
                .ForMember(p => p.Athlete, opt => opt.MapFrom(pic => pic.Participant.Athlete))
                .ForMember(p => p.Horse, opt => opt.MapFrom(pic => pic.Participant.Horse));
        }
    }
}
