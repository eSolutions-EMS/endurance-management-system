using Not.Application.Adapters.Behinds;
using Not.Application.Contexts;
using Not.Application.Ports.CRUD;
using NTS.Domain.Setup.Entities;
using NTS.Judge.Contexts;
using NTS.Judge.Setup.Competitions;

namespace NTS.Judge.Events;

public class CompetitionBehind : CrudBehind<Competition, CompetitionFormModel>
{
    private readonly EventParentContext _parentContext;
    private readonly IParentContext<Phase> _phasesContext;
    private readonly IParentContext<Participation> _participationsContext;

    public CompetitionBehind(IRepository<Competition> competitions, EventParentContext parentContext, IParentContext<Phase> phases, IParentContext<Participation> participations) : base(competitions, parentContext)
    {
        this._parentContext = parentContext;
        _phasesContext = phases;
        _participationsContext = participations;
    }

    protected override Competition CreateEntity(CompetitionFormModel model)
    {
        return Competition.Create(model.Name, model.Type, model.Ruleset, model.StartTime, model.CompulsoryThresholdMinutes);
    }

    protected override Competition UpdateEntity(CompetitionFormModel model)
    {
        return Competition.Update(
            model.Id,
            model.Name,
            model.Type,
            model.Ruleset,
            model.StartTime,
            model.CompulsoryThresholdMinutes,
            _phasesContext.Children,
            _participationsContext.Children);
    }
}
