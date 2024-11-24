using Not.Application.Behinds.Adapters;
using Not.Application.CRUD.Ports;
using NTS.Domain.Setup.Entities;
using NTS.Judge.Blazor.Setup.Combinations.Dot;

namespace NTS.Judge.Setup.Adapters;

public class CombinationBehind : CrudBehind<Combination, CombinationFormModel>
{
    public CombinationBehind(IRepository<Combination> repository)
        : base(repository) { }

    protected override Combination CreateEntity(CombinationFormModel model)
    {
        return Combination.Create(model.Number, model.Athlete, model.Horse, model.Tag);
    }

    protected override Combination UpdateEntity(CombinationFormModel model)
    {
        return Combination.Update(model.Id, model.Number, model.Athlete, model.Horse, model.Tag);
    }
}
