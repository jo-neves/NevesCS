namespace NevesCS.Static.Utils
{
    public static class NumericalConversionUtils
    {
        public static long EncodeDecimalAsLong(decimal source)
        {
            return (long)(source * 100);
        }

        public static decimal DecodeLongAsDecimal(long source)
        {
            return source / 100m;
        }
    }
}
