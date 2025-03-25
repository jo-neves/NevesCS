namespace NevesCS.Static.Utils.SqlBuilders
{
    public static class ConnectionStringBuilder
    {
        /// <summary>
        /// E.g.: "Data Source=c:\mydb.db;Version=3;Password=myPassword;"
        ///
        /// </summary>
        /// <param name="dataSource">The location.</param>
        /// <param name="failIfMissing">If the database file doesn't exist, the default behaviour is to create a new file.</param>
        /// <param name="readOnly">Read only connection.</param>
        /// <returns></returns>
        public static string BuildSQLiteAdoNet(
            string dataSource,
            bool failIfMissing,
            bool readOnly,
            string? password = default)
        {
            return $"Data Source=${dataSource};" +
                   (password == default ? "" : $"Password={password};") +
                   (readOnly ? "Mode=ReadOnly;" : "") +
                   (failIfMissing ? "Mode=ReadWrite;" : "");
        }

        public static string AppendToSQLiteAdoNetDataSource(string connectionString, string appender)
        {
            var part1 = connectionString.Split("Data Source=");
            var part2 = part1[1].Split(";");
            part2[0] += appender;

            return $"Data Source={part2[0]}" + ";" + string.Join(";", part2[1..]);
        }
    }
}
