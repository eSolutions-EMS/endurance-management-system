namespace Not.Extensions;

public static class DoubleExtension
{
    public static double RoundWholeNumberToTens(this double number)
    {
        return (double)Math.Floor((decimal)number / 10) * 10;
    }
}
