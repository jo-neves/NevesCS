using NevesCS.Static.Constants.Values;

namespace NevesCS.Static.Utils;

public static class CurrencyConversionUtils
{
    public static long DecimalToCents(this decimal value)
    {
        return decimal.ToInt64(
            decimal.Round(value * Decimals.OneHundred, MidpointRounding.AwayFromZero));
    }

    public static decimal CentsToDecimal(this long value)
    {
        return value / Decimals.OneHundred;
    }
}
