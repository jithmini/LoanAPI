using Microsoft.Data.SqlClient;
using System.Data;

namespace LOS_API.Data
{
    public class DBProvider:IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _sqlConnection;

        private const string connectionStringKey="SqlConnection";

        public DBProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            var con=configuration.GetConnectionString(connectionStringKey);

            _sqlConnection= new SqlConnection(con);
            _sqlConnection.Open();
        }

        public SqlConnection connection { 
            get {
                if (_sqlConnection.State != ConnectionState.Open)
                {
                    var connectionString = _configuration.GetConnectionString(connectionStringKey);
                    _sqlConnection.ConnectionString = connectionString;
                    _sqlConnection.Open();
                }
                return _sqlConnection; 
            } 
        }

        public void Dispose()
        {
            _sqlConnection?.Dispose();
            _sqlConnection?.Close();
            GC.SuppressFinalize(this);
        }
    }
}
