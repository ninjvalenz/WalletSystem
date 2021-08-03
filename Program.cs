using Microsoft.Extensions.Configuration;
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
using System.Web;
using System.Threading;

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

          

            Thread processUserRegistration = new Thread(new ThreadStart(UserRegistrations));
            Thread processMoneyTransaction = new Thread(new ThreadStart(MoneyTransactions));

            processUserRegistration.Start();
            processMoneyTransaction.Start();

        }

        private static void UserRegistrations()
        {
            do
            {
                Console.WriteLine("Processing UserRegistrations...");
                var userProcessResult = userSecurityService.ProcessQueue().Result;
                if (userProcessResult != null)
                    Console.WriteLine($"Done processing -- Error Msg: {userProcessResult.Message}; Processed: {userProcessResult.QueueResultViewModels.Count}");
            }
            while (true);
        }

        private static void MoneyTransactions()
        {
           
            do
            {
                Console.WriteLine("Processing MoneyTransactions...");
                var walletTransactionResult = walletTransactionService.ProcessQueue().Result;
                if (walletTransactionResult != null)
                    Console.WriteLine($"Done processing -- Error Msg: {walletTransactionResult.Message}; Processed: {walletTransactionResult.QueueResultViewModels.Count}");
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
