using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NevesCS.Static.Utils.Vendor.EntityFramework.ValueConverters
{
    public class DecimalToLongConverter : ValueConverter<decimal, long>
    {
        public DecimalToLongConverter()
            : base(
                  decimalValue => NumericalConversionUtils.EncodeDecimalAsLong(decimalValue),
                  longValue => NumericalConversionUtils.DecodeLongAsDecimal(longValue),
                  null)
        {
        }
    }
}
