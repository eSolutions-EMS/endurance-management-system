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

    public List<Start> Startlist { get; private set; } = new List<Start>();
    public IEnumerable<IGrouping<double, Start>> StartlistByPhase => Startlist.GroupBy(x => x.CurrentPhase.Length);


    public async Task Initialize()
    {
        var participations = await _participations.ReadAll();

        foreach (var participation in participations)
        {
            if (participation.Phases.OutTime != null && participation.IsNotQualified == false)
            {
                Startlist.Add(new Start(participation));
            }
        }
    }

    public void Subscribe(Func<Task> action)
    {
        throw new NotImplementedException();
    }
}
