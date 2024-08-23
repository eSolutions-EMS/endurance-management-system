using Not.Application.Ports.CRUD;
using NTS.Domain;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Objects;
using NTS.Judge.Blazor.Ports;

namespace NTS.Judge.Adapters.Behinds;
public class StartlistBehind : IStartlistBehind
{
    private readonly IRepository<Participation> _participations;

    public StartlistBehind(IRepository<Participation> participations)
    {
        _participations = participations;
    }

    public IEnumerable<PhaseStart> Startlist { get; private set; } = new List<PhaseStart>();
    public IEnumerable<IGrouping<double, PhaseStart>> StartlistByDistance => Startlist.GroupBy(x => x.Distance);


    public async Task Initialize()
    {
        var participations = await _participations.ReadAll();
        var startlist = new List<PhaseStart>();
        foreach (var participation in participations)
        {
            if (participation.Phases.OutTime != null && participation.IsNotQualified==false)
            {
                var phaseStart = new PhaseStart(participation);
                Startlist.ToList().Add(phaseStart);
            }
        }
    }

    public void Subscribe(Func<Task> action)
    {
        throw new NotImplementedException();
    }
}
