using System;

namespace EnduranceJudge.Core.Extensions;

public static class DoubleExtensions
{
    public static bool PreciseEquals(this double first, double second)
    {
        return Math.Abs(first - second) > 0;
    }
}
