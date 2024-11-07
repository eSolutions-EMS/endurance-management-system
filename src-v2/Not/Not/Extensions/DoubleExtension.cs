namespace Not.Extensions;
public static class DoubleExtension
{
    public static double FloorWholeNumberToTens(this double number)
    {
        return (double)Math.Floor((decimal)number / 10) * 10;
    }
}
