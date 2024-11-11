using System.Diagnostics;
using System.Timers;
using Not.Application.Adapters.Behinds;
using Not.Application.Ports.CRUD;
using Not.Exceptions;
using NTS.Domain;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Objects;
using NTS.Judge.Blazor.Ports;
using NTS.Judge.Factories;

namespace NTS.Judge.Adapters.Behinds;

public class StartlistBehind : ObservableBehind, IStartlistBehind
{
    private const double TimerTickInMilliseconds = 1000;
    private const int ExpirationTimeOfPreviousStartsInMinutes = 15;
    private readonly IRepository<Participation> _participationRepository;

    public StartlistBehind(IRepository<Participation> participations)
    {
        _participationRepository = participations;
    }

    public StartList Startlist { get; set; }
    public List<Start> Upcoming => Startlist.Upcoming;
    public List<Start> History => Startlist.History;

    protected override async Task<bool> PerformInitialization(params IEnumerable<object> arguments)
    {
        var participations = await _participationRepository.ReadAll();
        Startlist = new StartList();
        Startlist.AssignStarts(participations);
        GuardHelper.ThrowIfDefault(Startlist);
        return Upcoming.Any() && History.Any();
    }

    private void upcomingStartExpire(object? sender, ElapsedEventArgs e)
    {
        foreach (var start in Upcoming.ToList())
        {
            var now = new Timestamp(DateTime.Now);
            if (now - start.StartAt > TimeSpan.FromMinutes(ExpirationTimeOfPreviousStartsInMinutes))
            {
                Startlist.Expire(start);
                Startlist.OrderHistoryByAscending();
                EmitChange();
            }
        }
    }
}
