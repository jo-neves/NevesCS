using NevesCS.Static.Constants;

namespace NevesCS.Static.Utils.Sql.Triggers
{
    public static class DropTriggerSqlBuilder
    {
        public static string Drop(DatabaseVendorName databaseVendor, string triggerName)
        {
            return databaseVendor switch
            {
                DatabaseVendorName.Sqlite => SqlLite(triggerName),
                DatabaseVendorName.MySql => MySql(triggerName),
                DatabaseVendorName.PostgreSql => PostgreSql(triggerName),

                _ => throw new NotImplementedException(),
            };
        }

        public static string SqlLite(string triggerName)
        {
            return $"DROP TRIGGER IF EXISTS {triggerName};";
        }

        public static string MySql(string triggerName)
        {
            return $"DROP TRIGGER IF EXISTS {triggerName};";
        }

        public static string PostgreSql(string triggerName)
        {
            return $"DROP TRIGGER IF EXISTS  {triggerName};";
        }
    }
}
