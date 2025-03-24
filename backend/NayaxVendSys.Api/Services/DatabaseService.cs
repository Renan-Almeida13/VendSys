using System.Data;
using NayaxVendSys.Api.Models;
using Microsoft.Data.SqlClient;

namespace NayaxVendSys.Api.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> SaveDexMeterAsync(DexMeter dexMeter)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("SaveDexMeter", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Machine", dexMeter.Machine);
                    command.Parameters.AddWithValue("@DexDateTime", dexMeter.DexDateTime);
                    command.Parameters.AddWithValue("@MachineSerialNumber", dexMeter.MachineSerialNumber);
                    command.Parameters.AddWithValue("@ValueOfPaidVends", dexMeter.ValueOfPaidVends);

                    var returnParameter = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    await command.ExecuteNonQueryAsync();
                    
                    return (int)returnParameter.Value;
                }
            }
        }

        public async Task SaveDexLaneMetersAsync(List<DexLaneMeter> laneMeters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                foreach (var laneMeter in laneMeters)
                {
                    using (var command = new SqlCommand("SaveDexLaneMeter", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DexMeterId", laneMeter.DexMeterId);
                        command.Parameters.AddWithValue("@ProductIdentifier", laneMeter.ProductIdentifier);
                        command.Parameters.AddWithValue("@Price", laneMeter.Price);
                        command.Parameters.AddWithValue("@NumberOfVends", laneMeter.NumberOfVends);
                        command.Parameters.AddWithValue("@ValueOfPaidSales", laneMeter.ValueOfPaidSales);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
        }
    }
} 