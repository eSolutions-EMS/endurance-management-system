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
            StartTimeOverride = contestant.StartTimeOverride.Value.LocalDateTime.TimeOfDay;
        }
        IsUnranked = contestant.IsUnranked;
    }

    public int Id { get; set; }
    public TimeSpan? StartTimeOverride { get; set; }
    public Boolean  IsUnranked { get; set; }
}
