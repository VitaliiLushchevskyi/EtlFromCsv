using EtlFromCsv.Data;
using EtlFromCsv.Interfaces;
using EtlFromCsv.Services;
using EtlFromCsv.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IDbContext, DbContext>();
        services.AddSingleton<ICsvProcessor, CsvProcessor>();
        services.AddSingleton<ITripInserter, TripInserter>();
        services.Configure<CsvSettings>(context.Configuration.GetSection("CsvSettings"));
    })
    .Build();

var dbContext = host.Services.GetRequiredService<IDbContext>();
var csvProcessor = host.Services.GetRequiredService<ICsvProcessor>();
var bulkInserter = host.Services.GetRequiredService<ITripInserter>();
var csvSettings = host.Services.GetRequiredService<IOptions<CsvSettings>>().Value;

try
{
    var connection = dbContext.GetConnection();
    connection.Open();
    Console.WriteLine("Database connection established successfully.");


    var csvFilePath = csvSettings.CsvFilePath;
    var duplicatesFilePath = csvSettings.DuplicatesFilePath;

    var trips = csvProcessor.ReadTripsFromCsv(csvFilePath);

    var (uniqueTrips, duplicateTrips) = csvProcessor.RemoveDuplicates(trips);
    Console.WriteLine($"Found {duplicateTrips.Count()} duplicate trips.");

    csvProcessor.WriteDuplicatesToCsv(duplicateTrips, duplicatesFilePath);
    Console.WriteLine($"Remaining unique trips: {uniqueTrips.Count()}");

    await bulkInserter.BulkInsertTripsAsync(uniqueTrips);
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}