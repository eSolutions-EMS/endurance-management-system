using Not.Application.Adapters.Behinds;
using Not.Application.Ports.CRUD;
using Not.Exceptions;
using Not.Startup;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Objects;
using NTS.Domain.Core.Objects.Payloads;
using NTS.Judge.Blazor.Ports;

namespace NTS.Judge.Adapters.Behinds;

public class StartlistBehind : ObservableBehind, IStartlistUpcoming, IStartlistHistory
{
    readonly IRepository<Participation> _participationRepository;
    StartList? _startlist;

    public StartlistBehind(IRepository<Participation> participationRepository)
    {
        _participationRepository = participationRepository;
    }

    public IReadOnlyList<Start> Upcoming => _startlist?.Upcoming == null ? [] : _startlist.Upcoming;
    public IReadOnlyList<Start> History => _startlist?.History == null ? [] : _startlist.History;

    protected override async Task<bool> PerformInitialization(params IEnumerable<object> arguments)
    {
        var participations = await _participationRepository.ReadAll();
        var startlist = new StartList(participations, EmitChange);
        _startlist = startlist;
        return _startlist.Any();
    }
}
