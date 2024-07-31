using FluentAssertions;

using NevesCS.NonStatic.Builders.Sql.Statement;
using NevesCS.Tests.Mocks;

namespace NevesCS.Tests.NonStatic.Builders.Sql
{
    public class SqlStatementBuilderTests
    {
        [Fact]
        public void InsertIntoValues_WithColumnSelectors()
        {
            var now = DateTime.UtcNow;
            var table = new DataGame()
            {
                Name = "test",
                Developer = "test",
                ReleaseDate = now,
            };

            SqlStatementBuilder
                .Insert()
                .Into(table)
                .Values(table, t => t.Name, t => t.Developer, t => t.ReleaseDate)
                .Build()
                .Should()
                .ContainAll(
                    $"INSERT INTO {nameof(DataGame)} (",
                    nameof(DataGame.Name), nameof(DataGame.Developer), nameof(DataGame.ReleaseDate),
                    ")",
                    "VALUES (",
                    table.Name, table.Developer,
                    $"'{now}'",
                    ",");
        }
    }
}
