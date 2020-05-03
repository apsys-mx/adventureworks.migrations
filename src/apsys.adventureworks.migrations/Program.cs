using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace apsys.adventureworks.migrations
{
    public static class Program
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
                string provider = parameter["provider"].Trim().ToLower() ;

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


        private static IServiceProvider CreateServices(string connectionString, string provider)
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddFluentMigratorCore();
            serviceCollection.AddLogging(lb => lb.AddFluentMigratorConsole());
            switch (provider)
            {
                case "sqlserver":
                    serviceCollection.ConfigureRunner(rb => rb
                        .AddSqlServer2016()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(typeof(M001CreateAddressTable).Assembly).For.Migrations());
                    break;
                case "mysql":
                    serviceCollection.ConfigureRunner(rb => rb
                        .AddMySql5()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(typeof(M001CreateAddressTable).Assembly).For.Migrations());
                    break;
                default:
                    throw new ConfigurationErrorsException($"Invalid [{provider}]");
            }
            return serviceCollection.BuildServiceProvider(false);
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }

    enum ExitCode
    {
        Success = 0,
        UnknownError = 1
    }

    class CommandLineArgs : Dictionary<string, string>
    {
        private const string Pattern = @"\/(?<argname>\w+):(?<argvalue>.+)";
        private readonly Regex _regex = new Regex(Pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

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
