using System.Collections.Generic;

namespace EnduranceJudge.Domain.AggregateRoots.Import.Models.National;

public class NationalData
{
    public List<HorseExcelSchema> Horses { get; set; } = new();
}