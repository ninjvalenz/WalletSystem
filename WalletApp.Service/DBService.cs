using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Service.ConnectionStrings;
using WalletApp.Service.Interface;

namespace WalletApp.Service
{
    public class DBService : IDBService
    {
        ISequelConnection dbConnection;
        public DBService(ISequelConnection _dbConnection)
        {
            dbConnection = _dbConnection;
        }

        public async Task<DataTable> ExecuteQuery(string commandText, SqlParameter[] parameters, CommandType commandType)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection =
                    new SqlConnection(dbConnection.ConnectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;

                    if (parameters != null && parameters.Count() > 0)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.Add(param);
                        }
                    }

                    using(SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    
                }
            }

            return dataTable;
        }

        public async Task ExecuteNonQuery(string commandText, SqlParameter[] parameters, CommandType commandType)
        {
           
            using (SqlConnection connection =
                    new SqlConnection(dbConnection.ConnectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;

                    if (parameters != null && parameters.Count() > 0)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.Add(param);
                        }
                    }

                    await command.ExecuteNonQueryAsync();

                }
            }

            
        }
    }
}
