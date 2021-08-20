using AutoMapper;
using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Event.Participants;
using ImportParticipant = EnduranceJudge.Domain.Aggregates.Import.Participants.Participant;

namespace EnduranceJudge.Gateways.Persistence.Entities.Participants
{
    public class ParticipantEntityMaps : ICustomMapConfiguration
    {
        public void AddFromMaps(IProfileExpression profile)
        {
            profile.CreateMap<Participant, ParticipantEntity>();
            profile.CreateMap<ImportParticipant, ParticipantEntity>()
                .ForMember(x => x.HorseId, opt => opt.Condition(p => p.Horse != null))
                .ForMember(x => x.Horse, opt => opt.Condition(p => p.Horse != null))
                .ForMember(x => x.AthleteId, opt => opt.Condition(p => p.Athlete != null))
                .ForMember(x => x.Athlete, opt => opt.Condition(p => p.Athlete != null));
        }

        public void AddToMaps(IProfileExpression profile)
        {
            profile.CreateMap<ParticipantEntity, Participant>();
            profile.CreateMap<ParticipantEntity, ImportParticipant>();
            profile.CreateMap<ParticipantEntity, ParticipantDependantModel>()
                .MapMember(x => x.CategoryId, y => (int)y.Athlete.Category)
                .MapMember(x => x.HorseId, y => y.Horse.Id)
                .MapMember(x => x.AthleteId, y => y.Athlete.Id)
                .MapMember(x => x.Name, y => $"{y.Athlete.FirstName} {y.Athlete.LastName} - {y.Horse.Name}");
        }
    }
}
