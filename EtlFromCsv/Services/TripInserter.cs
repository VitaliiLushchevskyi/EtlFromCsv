using EtlFromCsv.Data;
using EtlFromCsv.Interfaces;
using EtlFromCsv.Models;
using System.Data.SqlClient;
using System.Data;

namespace EtlFromCsv.Services
{
    public sealed class TripInserter : ITripInserter
    {
        private readonly IDbContext _dbContext;

        public TripInserter(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task BulkInsertTripsAsync(IEnumerable<Trip> trips)
        {
            using var connection = _dbContext.GetConnection();
            await connection.OpenAsync();

            using var bulkCopy = new SqlBulkCopy((SqlConnection)connection)
            {
                DestinationTableName = "Trips"
            };

            var dataTable = ConvertTripsToDataTable(trips);

            bulkCopy.ColumnMappings.Add("TpepPickupDatetime", "tpep_pickup_datetime");
            bulkCopy.ColumnMappings.Add("TpepDropoffDatetime", "tpep_dropoff_datetime");
            bulkCopy.ColumnMappings.Add("PassengerCount", "passenger_count");
            bulkCopy.ColumnMappings.Add("TripDistance", "trip_distance");
            bulkCopy.ColumnMappings.Add("StoreAndFwdFlag", "store_and_fwd_flag");
            bulkCopy.ColumnMappings.Add("PULocationID", "PULocationID");
            bulkCopy.ColumnMappings.Add("DOLocationID", "DOLocationID");
            bulkCopy.ColumnMappings.Add("FareAmount", "fare_amount");
            bulkCopy.ColumnMappings.Add("TipAmount", "tip_amount");

            await bulkCopy.WriteToServerAsync(dataTable);
        }

        private static DataTable ConvertTripsToDataTable(IEnumerable<Trip> trips)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("TpepPickupDatetime", typeof(DateTime));
            dataTable.Columns.Add("TpepDropoffDatetime", typeof(DateTime));
            dataTable.Columns.Add("PassengerCount", typeof(int));
            dataTable.Columns.Add("TripDistance", typeof(float));
            dataTable.Columns.Add("StoreAndFwdFlag", typeof(string));
            dataTable.Columns.Add("PULocationID", typeof(int));
            dataTable.Columns.Add("DOLocationID", typeof(int));
            dataTable.Columns.Add("FareAmount", typeof(decimal));
            dataTable.Columns.Add("TipAmount", typeof(decimal));

            foreach (var trip in trips)
            {
                dataTable.Rows.Add(
                    trip.TpepPickupDatetime,
                    trip.TpepDropoffDatetime,
                    trip.PassengerCount,
                    trip.TripDistance,
                    trip.StoreAndFwdFlag,
                    trip.PULocationID,
                    trip.DOLocationID,
                    trip.FareAmount,
                    trip.TipAmount
                );
            }

            return dataTable;
        }
    }
}
