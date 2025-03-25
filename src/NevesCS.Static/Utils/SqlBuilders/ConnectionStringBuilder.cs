namespace NevesCS.Static.Utils.SqlBuilders
{
    public static class ConnectionStringBuilder
    {
        /// <summary>
        /// E.g.: "Data Source=c:\mydb.db;Version=3;Password=myPassword;"
        ///
        /// </summary>
        /// <param name="dataSource">The location.</param>
        /// <param name="storeGuidAsText">GUIDs are stored in a binary format by default.</param>
        /// <param name="failIfMissing">If the database file doesn't exist, the default behaviour is to create a new file.</param>
        /// <param name="readOnly">Read only connection.</param>
        /// <returns></returns>
        public static string BuildSqlLiteV3(
            string dataSource,
            bool storeGuidAsText,
            bool failIfMissing,
            bool readOnly,
            string? password = default)
        {
            return $"Data Source=${dataSource};Version=3;" +
                   (password == default ? "" : $"Password={password};") +
                   (storeGuidAsText ? "BinaryGUID=False;" : "") +
                   (readOnly ? "Read Only=True;" : "") +
                   (failIfMissing ? "FailIfMissing=True;" : "");
        }

        public static string AppendToSqlLiteV3DataSource(string connectionString, string appender)
        {
            var part1 = connectionString.Split("Data Source=");
            var part2 = part1[1].Split(";");
            part2[0] += appender;

            return $"Data Source={part2[0]}" + ";" + string.Join(";", part2[1..]);
        }
    }
}
