using Not.Application.Behinds.Adapters;
using Not.Application.CRUD.Ports;
using Not.Extensions;
using NTS.Domain.Setup.Entities;
using NTS.Judge.Blazor.Pages.Setup.Contestants;
using NTS.Judge.Core.Behinds;

namespace NTS.Judge.Setup.Adapters;

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
        return Participation.Create(
            newStart,
            model.IsNotRanked,
            model.Combination,
            model.MaxSpeedOverride
        );
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
