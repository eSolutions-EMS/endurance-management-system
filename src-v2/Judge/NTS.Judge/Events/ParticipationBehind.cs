using Not.Application.Adapters.Behinds;
using Not.Application.Ports.CRUD;
using Not.Extensions;
using NTS.Domain.Setup.Entities;
using NTS.Judge.Blazor.Pages.Setup.Contestants;
using NTS.Judge.Contexts;

namespace NTS.Judge.Events;

public class ParticipationBehind : CrudBehind<Participation, ParticipationFormModel>
{
    public ParticipationBehind(
        IRepository<Participation> participations,
        CompetitionParentContext parentContext
    )
        : base(participations, parentContext) { }

    protected override Participation CreateEntity(ParticipationFormModel model)
    {
        var newStart = model.StartTimeOverride?.ToDateTimeOffset();
        return Participation.Create(newStart, model.IsNotRanked, model.Combination, model.MaxSpeedOverride);
    }

    protected override Participation UpdateEntity(ParticipationFormModel model)
    {
        var newStart = model.StartTimeOverride?.ToDateTimeOffset();
        return Participation.Update(
            model.Id,
            newStart,
            model.IsNotRanked,
            model.Combination,
            model.MaxSpeedOverride
        );
    }
}
