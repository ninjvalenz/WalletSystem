using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Model.DomainModel;
using WalletApp.Model.ViewModel;
using WalletApp.Model.ViewModel.Exceptions;
using WalletApp.Service.ConnectionStrings;
using WalletApp.Service.Helper;
using WalletApp.Service.Interface;

namespace WalletApp.Service
{
    public class UserSecurityService : IUserSecurityService
    {
        //ISequelConnection dbConnection;
        IDBService dBService;
        public UserSecurityService(IDBService _dBService)
        {
           
            dBService = _dBService;
        }
        public async Task<AuthenticatedLoginViewModel> AuthenticateUser(string login, string password)
        {
            AuthenticatedLoginViewModel authenticatedLoginViewModel = new AuthenticatedLoginViewModel();

            try
            {

                var domainResult = await dBService.ExecuteQuery("AuthenticateLogin",
                    new SqlParameter[]
                    {
                        new SqlParameter() { ParameterName = "Login", Value = login },
                        new SqlParameter() { ParameterName = "Password", Value = password}
                    }, CommandType.StoredProcedure);

                if (domainResult != null && domainResult.Rows != null && domainResult.Rows.Count > 0)
                    authenticatedLoginViewModel = domainResult.ToViewModel();
                else throw new UnauthorizedUserException();
            }
            catch (Exception ex)
            {
                authenticatedLoginViewModel.Message = ex.Message;
            }

            return authenticatedLoginViewModel;
        }

        public async Task<RegisterUserViewModel> RegisterUser(string login, string password)
        {
            RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel();

            try
            {
                //using (SqlConnection connection =
                //  new SqlConnection(dbConnection.ConnectionString))
                //{
                //    await connection.OpenAsync();

                //    using (SqlCommand command = new SqlCommand("RegisterUser", connection))
                //    {
                //        command.CommandType = CommandType.StoredProcedure;
                //        command.Parameters.AddWithValue("@Login", login);
                //        command.Parameters.AddWithValue("@Password", password);

                //        using (SqlDataReader sqlDataReader = await command.ExecuteReaderAsync())
                //        {
                //            if (await sqlDataReader.ReadAsync())
                //                registerUserViewModel = sqlDataReader.GetFieldValue<Guid>("UserSecurityID").ToViewModel();
                //        }


                //    }
                //}

                var domainResult = await dBService.ExecuteQuery("RegisterUser",
                    new SqlParameter[]
                    {
                        new SqlParameter() { ParameterName = "Login", Value = login },
                        new SqlParameter() { ParameterName = "Password", Value = password}
                    }, CommandType.StoredProcedure);

                if (domainResult != null && domainResult.Rows != null && domainResult.Rows.Count > 0 && domainResult.Rows[0][0] != DBNull.Value)
                    registerUserViewModel = domainResult.RegisterUserToViewModel();
                else
                    throw new UnableToRegisterUserException();

            }
            catch(Exception ex)
            {
                registerUserViewModel.Message = ex.Message;
            }

            return registerUserViewModel;

        }
    }
}
