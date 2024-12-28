using EtlFromCsv.Models;

namespace EtlFromCsv.Interfaces
{
    public interface ITripInserter
    {
        Task BulkInsertTripsAsync(IEnumerable<Trip> trips);
    }
}
