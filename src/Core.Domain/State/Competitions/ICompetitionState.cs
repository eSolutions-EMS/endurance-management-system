using System;
using Core.Domain.Enums;
using Core.Models;

namespace Core.Domain.State.Competitions;

public interface ICompetitionState : IIdentifiable
{
    CompetitionType Type { get; }
    string Name { get; }
    DateTime StartTime { get; }
    string FeiCategoryEventNumber { get; }
    string FeiScheduleNumber { get; }
    string Rule { get; }
    string EventCode { get; }
}
