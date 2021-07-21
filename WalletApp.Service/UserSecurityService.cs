﻿using System;
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
        ISequelConnection dbConnection;
        public UserSecurityService(ISequelConnection _dbConnection)
        {
            dbConnection = _dbConnection;
        }
        public async Task<AuthenticatedLoginViewModel> AuthenticateUser(string login, string password)
        {
            AuthenticatedLoginViewModel authenticatedLoginViewModel = new AuthenticatedLoginViewModel();

            try
            {

                using (SqlConnection connection = 
                    new SqlConnection(dbConnection.ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("AuthenticateLogin", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", password);

                        List<AuthenticatedLogin> userResult = new List<AuthenticatedLogin>();

                        using (SqlDataReader sqlDataReader = await command.ExecuteReaderAsync())
                        {
                            while(await sqlDataReader.ReadAsync())
                            {
                                var userInstance = new AuthenticatedLogin();
                                userInstance.Id = sqlDataReader.GetFieldValue<Guid>("Id");
                                userInstance.Login = sqlDataReader.GetFieldValue<string>("Login");
                                userInstance.AccountNumber = sqlDataReader.GetFieldValue<long>("AccountNumber");

                                userResult.Add(userInstance);
                            }
                        }

                        if (userResult.Count > 0)
                            authenticatedLoginViewModel = userResult.ToViewModel();
                        else throw new UnauthorizedUserException();
                    }
                }
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
                using (SqlConnection connection =
                  new SqlConnection(dbConnection.ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("RegisterUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader sqlDataReader = await command.ExecuteReaderAsync())
                        {
                            if (await sqlDataReader.ReadAsync())
                                registerUserViewModel = sqlDataReader.GetFieldValue<Guid>("UserSecurityID").ToViewModel();
                        }


                    }
                }
            }
            catch(Exception ex)
            {
                registerUserViewModel.Message = ex.Message;
            }

            return registerUserViewModel;

        }
    }
}
