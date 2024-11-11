using System.Diagnostics;
using System.Timers;
using Not.Application.Adapters.Behinds;
using Not.Application.Ports.CRUD;
using Not.Exceptions;
using NTS.Domain;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Objects;
using NTS.Domain.Objects;
using NTS.Judge.Blazor.Ports;
using NTS.Judge.Factories;

namespace NTS.Judge.Adapters.Behinds;

public class StartlistBehind : ObservableBehind, IStartlistBehind
{
    const double TIMER_TICK_IN_MILLISECONDS = 1000;
    const int EXPIRATION_TIME_OF_PREVIOUS_STARTS_IN_MINUTES = 15;

    readonly IRepository<Participation> _participationRepository;

    public StartlistBehind(IRepository<Participation> participations)
    {
        _participationRepository = participations;
        var startlist = new StartList();
        Startlist = startlist;
        _timer = TimerFactory.CreateTimer(TIMER_TICK_IN_MILLISECONDS, upcomingStartExpire, () => true);
    }

    public StartList Startlist { get; set; }
    public List<Start> Upcoming => Startlist.Upcoming;
    public List<Start> History => Startlist.History;
    public System.Timers.Timer _timer { get; set; }

    protected override async Task<bool> PerformInitialization(params IEnumerable<object> arguments)
    {
        var participations = await _participationRepository.ReadAll();
        Startlist.AssignStarts(participations);
        GuardHelper.ThrowIfDefault(Startlist);
        return Upcoming.Any() && History.Any();
    }

    void upcomingStartExpire(object? sender, ElapsedEventArgs e)
    {
        foreach (var start in Upcoming.ToList())
        {
            var now = new Timestamp(DateTime.Now);
            if (now - start.StartAt > TimeSpan.FromMinutes(EXPIRATION_TIME_OF_PREVIOUS_STARTS_IN_MINUTES))
            {
                Startlist.Expire(start);
                Startlist.OrderHistoryByAscending();
                EmitChange();
            }
        }
    }
}
