namespace NevesCS.Static.Utils;

public static class CurrencyConversionUtils
{
    public static long DecimalToCents(this decimal value)
    {
        return decimal.ToInt64(decimal.Round(value * 100m, MidpointRounding.AwayFromZero));
    }

    public static decimal CentsToDecimal(this long value)
    {
        return value / 100m;
    }
}
