using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using WalletApp.Service.ConnectionStrings;

namespace WalletApp.Service.ConnectionStrings
{
    public class DevSqlConnection : ISequelConnection
    {
        private string _connectionString = "";
        public DevSqlConnection(IConfiguration config)
        {
            _connectionString = config.GetSection("ConnectionStrings")["connectionStringDev"];
        }

        public string ConnectionString => _connectionString;
    }
}
