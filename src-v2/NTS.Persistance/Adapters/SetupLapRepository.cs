using NTS.Domain.Setup.Entities;
using NTS.Persistence.Setup;

namespace NTS.Persistence.Adapters;

public class SetupLapRepository : SetRepository<Lap, SetupState>
{
    public SetupLapRepository(IStore<SetupState> store) : base(store)
    {
        state = store.Load().Result;
    }

    private SetupState state { get; set; }
    public override Task<Lap> Update(Lap entity)
    {
        state.Event.Competitions
            .Where(competition => competition.Phases.Any(phase => phase.SelectedLap.Id == entity.Id))
            .SelectMany(competition => competition.Phases
                .Where(phase => phase.SelectedLap.Id == entity.Id)
                .Select(phase => new { competition, phase }))
            .ToList()
            .ForEach(item =>
            {
                item.phase.SetSelectedLap(entity);
                item.competition.Update(item.phase);
            });
        return base.Update(entity);
    }
    public override Task<Lap> Delete(Lap entity)
    {
        state.Event.Competitions
            .Where(competition => competition.Phases.Any(phase => phase.SelectedLap.Id == entity.Id))
            .SelectMany(competition => competition.Phases
                .Where(phase => phase.SelectedLap.Id == entity.Id)
                .Select(phase => new { competition, phase }))
            .ToList()
            .ForEach(item =>
            {
                item.phase.SetSelectedLap(null);
                item.competition.Update(item.phase);
            });
        return base.Delete(entity);
    }
}