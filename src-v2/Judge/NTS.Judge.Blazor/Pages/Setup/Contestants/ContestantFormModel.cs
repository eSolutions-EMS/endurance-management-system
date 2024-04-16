using NTS.Domain.Setup.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Judge.Blazor.Pages.Setup.Contestants;
public class ContestantFormModel
{
    public ContestantFormModel()
    {
        // mock data for testing
        StartTime = TimeSpan.Zero;
    }
    public ContestantFormModel(Contestant contestant)
    {
        Id = contestant.Id;
        TimeSpan? startTime = contestant.StartTimeOverride.DateTime.TimeOfDay;
        StartTime = startTime;
    }

    public int Id { get; set; }
    public TimeSpan? StartTime { get; set; }
}
