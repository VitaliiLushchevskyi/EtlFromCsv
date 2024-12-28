using System.Data.SqlClient;

namespace EtlFromCsv.Data
{
    public interface IDbContext
    {
        SqlConnection GetConnection();
    }
}
