using NevesCS.Static.Constants;

namespace NevesCS.Static.Utils.SqlBuilders.Functions
{
    public static class SqlCurrentDatetimeFunction
    {
        public const string Sqlite = "DATETIME('now')";

        public const string MySQL = "UTC_TIMESTAMP()";

        public const string PostgreSQL = "timezone('utc', now())";

        public static string BuildSqlString(DatabaseVendorName databaseVendor)
        {
            return databaseVendor switch
            {
                DatabaseVendorName.Sqlite => Sqlite,
                DatabaseVendorName.MySql => MySQL,
                DatabaseVendorName.PostgreSql => PostgreSQL,

                _ => throw new NotImplementedException(databaseVendor.ToString()),
            };
        }
    }
}
