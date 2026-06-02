using NevesCS.Static.Utils;

namespace NevesCS.Static.Extensions
{
    public static class LongExtensions
    {
        public static decimal DecodeAsDecimal(this long source)
        {
            return NumericalConversionUtils.DecodeLongAsDecimal(source);
        }
    }
}
