using CsvHelper;
using CsvHelper.Configuration;
using EtlFromCsv.Interfaces;
using EtlFromCsv.Models;
using System.Globalization;

namespace EtlFromCsv.Services
{
    public class CsvProcessor : ICsvProcessor
    {
        public IEnumerable<Trip> ReadTripsFromCsv(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"CSV file not found at path: {filePath}");
            }

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { TrimOptions = TrimOptions.Trim });


            var records = csv.GetRecords<Trip>().ToList();

            foreach (var trip in records)
            {
                trip.StoreAndFwdFlag = trip.StoreAndFwdFlag switch
                {
                    "N" => "No",
                    "Y" => "Yes",
                    _ => trip.StoreAndFwdFlag
                };
            }

            return records;
        }
        public (IEnumerable<Trip> UniqueTrips, IEnumerable<Trip> DuplicateTrips) RemoveDuplicates(IEnumerable<Trip> trips)
        {
            var groupedTrips = trips.GroupBy(t => new
            {
                t.TpepPickupDatetime,
                t.TpepDropoffDatetime,
                t.PassengerCount
            });

            var duplicates = groupedTrips.Where(g => g.Count() > 1).SelectMany(g => g.Skip(1));
            var uniqueTrips = groupedTrips.Select(g => g.First());

            return (uniqueTrips, duplicates);
        }

        public void WriteDuplicatesToCsv(IEnumerable<Trip> duplicates, string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(filePath))
            {
                using (File.Create(filePath)){}
            }

            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
            csv.WriteRecords(duplicates);
            //using var writer = new StreamWriter(filePath);
            //using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
            //csv.WriteRecords(duplicates);
        }
    }
}
