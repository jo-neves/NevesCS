using FluentAssertions;

using NevesCS.Static.Utils.SqlBuilders;

namespace NevesCS.Tests.Static.SqlBuilders
{
    public class ConnectionStringBuilderTests
    {
        [Fact]
        public void AppendToSqlLiteV3DataSource_Passes()
        {
            const string originalConnectionString = "Data Source=c:\\mydb.db;Version=3;Password=myPassword;";

            ConnectionStringBuilder.AppendToSqlLiteV3DataSource(originalConnectionString, "_123")
                .Should()
                .Be("Data Source=c:\\mydb.db_123;Version=3;Password=myPassword;");
        }
    }
}
