using EtlFromCsv.Models;

namespace EtlFromCsv.Interfaces
{
    public interface ICsvProcessor
    {
        IEnumerable<Trip> ReadTripsFromCsv(string filePath);
        (IEnumerable<Trip> UniqueTrips, IEnumerable<Trip> DuplicateTrips) RemoveDuplicates(IEnumerable<Trip> trips);
        void WriteDuplicatesToCsv(IEnumerable<Trip> duplicates, string filePath);
    }
}
