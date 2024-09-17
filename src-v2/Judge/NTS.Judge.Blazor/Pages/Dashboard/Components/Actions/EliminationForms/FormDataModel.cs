using NTS.Domain.Core.Aggregates.Participations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Judge.Blazor.Pages.Dashboard.Components.Actions.EliminationForms;
public class FormData
{
    public string EliminationCode { get; set; }
    public int TandemNumber { get; set; }
    public string? Reason { get; set; }
    public List<FTQCodes> Codes { get; set; } = new List<FTQCodes>();

    public FormData(string eliminationCode, int number, string? reason, IEnumerable<string> selected_values)
    {
        EliminationCode = eliminationCode;
        TandemNumber = number;
        Reason = reason;
        foreach (var value in selected_values)
        {
            Codes.Add((FTQCodes)Enum.Parse(typeof(FTQCodes), value));
        }
    }
}
