using System;
using System.Collections.Generic;
using System.Text;

namespace WalletApp.Service.ConnectionStrings
{
    public interface ISequelConnection
    {
        public string ConnectionString { get; }
    }
}
