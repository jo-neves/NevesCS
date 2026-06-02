using CsvHelper;
using CsvHelper.Configuration;

using System.Globalization;
using System.Text;

namespace NevesCS.Static.Utils.Vendor
{
    public static class CsvHelperUtils
    {
        public static async Task WriteCsvFileAsync<T>(
            string filePath,
            IEnumerable<T> records,
            bool append,
            CancellationToken cancellationToken = default)
        {
            await using var streamWriter = new StreamWriter(filePath, append, encoding: Encoding.UTF8);

            await using var csvWriter = new CsvWriter(
                streamWriter,
                new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = !append,
                });

            await csvWriter.WriteRecordsAsync(records, cancellationToken);

            await csvWriter.FlushAsync();
            await streamWriter.FlushAsync();
        }

        public static async Task<List<T>> ReadCsvFileAsync<T>(string filePath, CancellationToken cancellationToken = default)
        {
            using var streamReader = new StreamReader(filePath, encoding: Encoding.UTF8);
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            return await csvReader.GetRecordsAsync<T>(cancellationToken).ToListAsync(cancellationToken);
        }
    }
}
