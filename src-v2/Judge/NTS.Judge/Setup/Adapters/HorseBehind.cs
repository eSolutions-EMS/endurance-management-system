using Not.Application.Behinds.Adapters;
using Not.Application.CRUD.Ports;
using NTS.Domain.Setup.Entities;
using NTS.Judge.Blazor.Pages.Setup.Horses;

namespace NTS.Judge.Setup.Adapters;

public class HorseBehind : CrudBehind<Horse, HorseFormModel>
{
    public HorseBehind(IRepository<Horse> repository)
        : base(repository) { }

    protected override Horse CreateEntity(HorseFormModel model)
    {
        return Horse.Create(model.Name, model.FeiId);
    }

    protected override Horse UpdateEntity(HorseFormModel model)
    {
        return Horse.Update(model.Id, model.Name, model.FeiId);
    }
}
