using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WalletApp.Model.ViewModel;
using WalletApp.Service.ConnectionStrings;
using WalletApp.Service.Helper;
using WalletApp.Service.Interface;

namespace WalletApp.Service
{
    
    public class UserWalletAccountService : IUserWalletAccountService
    {
        ISequelConnection dbConnection;
        public UserWalletAccountService(ISequelConnection _dbConnection)
        {
            dbConnection = _dbConnection;

        }

        public async Task<RegisterWalletViewModel> RegisterWallet(Guid userSecurityId)
        {
            RegisterWalletViewModel registerWalletViewModel = new RegisterWalletViewModel();

            try
            {
                using (SqlConnection connection =
               new SqlConnection(dbConnection.ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("RegisterWallet", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserSecurityId", userSecurityId);

                        using (SqlDataReader sqlDataReader = await command.ExecuteReaderAsync())
                        {
                            while (await sqlDataReader.ReadAsync())
                            {
                                registerWalletViewModel = sqlDataReader.GetFieldValue<long>("AccountNumber").ToViewModel();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                registerWalletViewModel.Message = ex.Message;
            }
           
            return registerWalletViewModel;
        }
    }
}
