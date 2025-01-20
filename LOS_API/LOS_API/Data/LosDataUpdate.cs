using LOS_API.Models;
using System.Data;

namespace LOS_API.Data
{
    public class LosDataUpdate
    {
        private readonly IConfiguration _configuration;

        public LosDataUpdate(IConfiguration configuration,DBProvider dBProvider)
        {
            _configuration = configuration;
        }

        public async Task<bool> updateLosStatus(LosData losData)
        {
            using (var dbprovider = new DBProvider(_configuration))
            {
                using (var transaction = dbprovider.connection.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = dbprovider.connection.CreateCommand())
                        {
                            cmd.Transaction = transaction;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "SP_LN_UPDATE_LOS_STATUS_HNB";
                            cmd.Parameters.AddWithValue("@losNum", losData.LOSNumber);
                            cmd.Parameters.AddWithValue("@status", losData.Status);
                            cmd.Parameters.AddWithValue("@grantDate", losData.GrantDate);
                            await cmd.ExecuteNonQueryAsync();
                        }
                        transaction.CommitAsync();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


    }
}
