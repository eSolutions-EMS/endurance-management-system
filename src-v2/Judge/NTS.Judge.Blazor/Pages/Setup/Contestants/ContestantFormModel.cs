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
        StartTimeOverride = null;
    }
    public ContestantFormModel(Contestant contestant)
    {
        Id = contestant.Id;
        if (contestant.StartTimeOverride != null)
        {
            TimeSpan? startTime = contestant.StartTimeOverride.Value.DateTime.TimeOfDay;
            StartTimeOverride = startTime;
        }
        else
        {
            StartTimeOverride = null;
        }
        Unranked = contestant.Unranked;
    }

    public int Id { get; set; }
    public TimeSpan? StartTimeOverride { get; set; }
    public Boolean  Unranked { get; set; }
}
