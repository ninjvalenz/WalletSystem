using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Service.Interface
{
    public interface IDBService
    {
        public Task<DataTable> ExecuteQuery(string commandText, SqlParameter[] parameters, CommandType commandType);
        public Task ExecuteNonQuery(string commandText, SqlParameter[] parameters, CommandType commandType);
    }
}
