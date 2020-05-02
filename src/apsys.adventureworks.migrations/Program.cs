using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentMigrator.Runner;
using System.Net.Http.Headers;

namespace apsys.adventureworks.migrations
{
    public class Program
    {
        static int Main(string[] args)
        {
            try
            {
                CommandLineArgs parameter = new CommandLineArgs();
                if (!parameter.ContainsKey("cnn"))
                    throw new ArgumentException("No [cnn] parameter received. You need pass the connection string in order to execute the migrations");

                if (!parameter.ContainsKey("provider"))
                    throw new ArgumentException("No [provider] parameter received. You need pass the database provider identifier in order to execute the migrations");

                string connectionString = parameter["cnn"];
                string provider = parameter["provider"];

                var serviceProvider = CreateServices(connectionString, provider);
                using (var scope = serviceProvider.CreateScope())
                    UpdateDatabase(scope.ServiceProvider);

                return (int)ExitCode.Success;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error updating the database schema: {ex.Message}");
                return (int)ExitCode.UnknownError;
            }
        }
        /// <summary>
        /// Configure the dependency injection services
        /// </sumamry>
        private static IServiceProvider CreateServices(string connectionString, string dataBase)
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddFluentMigratorCore();
            serviceCollection.AddLogging(lb => lb.AddFluentMigratorConsole());
            if (dataBase == "sqlserver")
            {
                serviceCollection.ConfigureRunner(rb => rb
                    .AddSqlServer2016()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(M001CreateAddressTable).Assembly).For.Migrations());
            }
            if (dataBase == "mysql")
            {
                serviceCollection.ConfigureRunner(rb => rb
                    .AddMySql5()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(M001CreateAddressTable).Assembly).For.Migrations());
            }
            return serviceCollection.BuildServiceProvider(false);
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }

    /// <summary>
    /// Enumerate the exit codes
    /// </summary>
    enum ExitCode
    {
        Success = 0,
        UnknownError = 1
    }

    /// <summary>
    /// Dictionary with input parameters of console application
    /// </summary>
    class CommandLineArgs : Dictionary<string, string>
    {
        private const string Pattern = @"\/(?<argname>\w+):(?<argvalue>.+)";
        private readonly Regex _regex = new Regex(Pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Determine if the user pass at least one valid parameter
        /// </summary>
        /// <returns></returns>
        public bool ContainsValidArguments()
        {
            return (this.ContainsKey("cnn"));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CommandLineArgs()
        {
            var args = Environment.GetCommandLineArgs();
            foreach (var match in args.Select(arg => _regex.Match(arg)).Where(m => m.Success))
            {
                try
                {
                    this.Add(match.Groups["argname"].Value, match.Groups["argvalue"].Value);
                }
                catch 
                {
                    // Nothing to do
                }
            }
        }
    }
}
