using FluentAssertions;

using NevesCS.Static.Utils.SqlBuilders;

namespace NevesCS.Tests.Static.SqlBuilders
{
    public class ConnectionStringBuilderTests
    {
        [Fact]
        public void AppendToSQLiteAdoNetDataSource_Passes()
        {
            const string originalConnectionString = "Data Source=c:\\mydb.db;Password=myPassword;";

            ConnectionStringBuilder.AppendToSQLiteAdoNetDataSource(originalConnectionString, "_123")
                .Should()
                .Be("Data Source=c:\\mydb.db_123;Password=myPassword;");
        }
    }
}
