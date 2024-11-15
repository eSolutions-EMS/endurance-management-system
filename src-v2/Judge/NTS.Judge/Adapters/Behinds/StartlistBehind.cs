using Not.Application.Adapters.Behinds;
using Not.Application.Ports.CRUD;
using Not.Exceptions;
using Not.Startup;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Objects;
using NTS.Domain.Core.Objects.Payloads;
using NTS.Judge.Blazor.Ports;

namespace NTS.Judge.Adapters.Behinds;

public class StartlistBehind : ObservableBehind, IStartlistBehind
{
    readonly IRepository<Participation> _participationRepository;

    public StartlistBehind(IRepository<Participation> participationRepository)
    {
        _participationRepository = participationRepository;
        var startlist = new StartList(EmitChange);
        Startlist = startlist;
    }

    StartList Startlist { get; set; }
    public IReadOnlyList<Start> Upcoming => Startlist.Upcoming;
    public IReadOnlyList<Start> History => Startlist.History;

    protected override async Task<bool> PerformInitialization(params IEnumerable<object> arguments)
    {
        var participations = await _participationRepository.ReadAll();
        Startlist.AssignStarts(participations);
        return Startlist.Starts.Any();
    }
}
