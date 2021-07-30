﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using WalletApp.Model.Enums;
using WalletApp.Service;
using WalletApp.Service.ConnectionStrings;
using WalletApp.Service.Interface;

namespace WalletApp
{
    class Program
    {
        static IConfigurationRoot _configuration;
        static IUserSecurityService userSecurityService;
        static IUserWalletAccountService userWalletAccountService;
        static IWalletTransactionService walletTransactionService;

        static async Task Main(string[] args)
        {

            ConfigureDependecyInjection();

            do
            {
                var userProcessResult = await userSecurityService.ProcessQueue();
            }
            while (true);
            
            
            
        }

        private static void ConfigureDependecyInjection()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();
            var serviceCollection = new ServiceCollection()
               .AddLogging()
               .AddSingleton<IConfiguration>(_configuration);

            serviceCollection.AddTransient(typeof(ISequelConnection), typeof(DevSqlConnection));
            serviceCollection.AddTransient(typeof(IDBService), typeof(DBService));
            serviceCollection.AddTransient(typeof(IUserSecurityService), typeof(UserSecurityService));
            serviceCollection.AddTransient(typeof(IUserWalletAccountService), typeof(UserWalletAccountService));
            serviceCollection.AddTransient(typeof(IWalletTransactionService), typeof(WalletTransactionService));

            var serviceProvider = serviceCollection.BuildServiceProvider();

            userSecurityService = serviceProvider.GetService<IUserSecurityService>();
            userWalletAccountService = serviceProvider.GetService<IUserWalletAccountService>();
            walletTransactionService = serviceProvider.GetService<IWalletTransactionService>();
        }
    }
}
