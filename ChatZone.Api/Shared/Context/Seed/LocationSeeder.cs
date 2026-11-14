using System.Data;
using ChatZone.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Shared.Context.Seed;

//Fills Country/City/Language from Seed/Data/*.tsv
//Adds only missing rows
public static class LocationSeeder
{
    private static readonly string DataFolder = Path.Combine(AppContext.BaseDirectory, "Shared", "Context", "Seed", "Data");

    public static async Task SeedAsync(ChatZoneDbContext dbContext, ILogger logger, CancellationToken cancellationToken = default)
    {
        var existingCountries = await dbContext.Countries.Select(x => x.IsoCode).ToListAsync(cancellationToken);
        var newCountries = ReadRows("countries.tsv")
            .Where(x => !existingCountries.Contains(x[0]))
            .Select(x => new Country { IsoCode = x[0], Name = x[1] })
            .ToList();
        dbContext.Countries.AddRange(newCountries);

        var existingLanguages = await dbContext.Languages.Select(x => x.IsoCode).ToListAsync(cancellationToken);
        var newLanguages = ReadRows("languages.tsv")
            .Where(x => !existingLanguages.Contains(x[0]))
            .Select(x => new Language { IsoCode = x[0], Name = x[1] })
            .ToList();
        dbContext.Languages.AddRange(newLanguages);

        await dbContext.SaveChangesAsync(cancellationToken);

        var countryIds = await dbContext.Countries.ToDictionaryAsync(x => x.IsoCode, x => x.IdCountry, cancellationToken);
        var existingCities = (await dbContext.Cities.Select(x => x.GeoNameId).ToListAsync(cancellationToken)).ToHashSet();

        var cities = new DataTable();
        cities.Columns.Add(nameof(City.IdCountry), typeof(int));
        cities.Columns.Add(nameof(City.Name), typeof(string));
        cities.Columns.Add(nameof(City.Population), typeof(int));
        cities.Columns.Add(nameof(City.GeoNameId), typeof(int));
        foreach (var x in ReadRows("cities.tsv"))
        {
            var geoNameId = int.Parse(x[0]);
            if (existingCities.Contains(geoNameId) || !countryIds.TryGetValue(x[1], out var idCountry)) continue;
            cities.Rows.Add(idCountry, x[2], int.Parse(x[3]), geoNameId);
        }

        await BulkInsertAsync(dbContext, nameof(City), cities, cancellationToken);

        if (newCountries.Count + newLanguages.Count + cities.Rows.Count > 0)
            logger.LogInformation("Location seed: added {Countries} countries, {Cities} cities, {Languages} languages",
                newCountries.Count, cities.Rows.Count, newLanguages.Count);
    }

    private static async Task BulkInsertAsync(ChatZoneDbContext dbContext, string table, DataTable rows, CancellationToken cancellationToken)
    {
        if (rows.Rows.Count == 0) return;

        var connection = (SqlConnection)dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open) await connection.OpenAsync(cancellationToken);
        
        using var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.UseInternalTransaction, null);
        bulkCopy.DestinationTableName = $"[{table}]";
        bulkCopy.BatchSize = 0;
        bulkCopy.BulkCopyTimeout = 300;
        foreach (DataColumn column in rows.Columns) bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);

        await bulkCopy.WriteToServerAsync(rows, cancellationToken);
    }

    private static IEnumerable<string[]> ReadRows(string fileName) =>
        File.ReadLines(Path.Combine(DataFolder, fileName))
            .Skip(1)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Split('\t'));
}
