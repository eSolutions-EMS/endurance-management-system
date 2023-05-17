using Core.Domain.Enums;
using Core.Models;
using System;

namespace Core.Domain.State.Competitions;

public interface ICompetitionState : IIdentifiable
{
    CompetitionType Type { get; }
    string Name { get; }
    DateTime StartTime { get; }
}
