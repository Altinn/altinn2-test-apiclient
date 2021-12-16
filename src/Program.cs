using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using CommandLine;
using Altinn.TestClient.Handlers;
using Altinn.TestClient.Testdata;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;

namespace Altinn.TestClient.Program
{
    class Program
    {
        public class Options
        {
            [Option('v', "verbose", Required = false, HelpText = "Print verbose")]
            public bool Verbose { get; set; }
        }

        static async Task<int> Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options =>
                {
                    // TODO Add argument handling
                });

            
            await CreateHostBuilder().RunConsoleAsync();

            return Environment.ExitCode;
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .UseEnvironment(Environments.Development)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<ConsoleHostedService>();
                })
                .ConfigureAppConfiguration((hostContext, builder) =>
                {
                    builder.AddEnvironmentVariables();
                    builder.AddUserSecrets<Program>();
                });
        }
    }

    internal class ConsoleHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IConfiguration _config;

        public ConsoleHostedService( ILogger<ConsoleHostedService> logger, IHostApplicationLifetime appLifetime, IConfiguration config)
        {
            _logger = logger; // TODO Add logging to HTTP traffic and exceptions.
            _appLifetime = appLifetime;
            _config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var authenticationHandler = new AuthenticationHandler(
                _config["Maskinporten:ClientId"],
                _config["Altinn:EnterpriseUser:Username"], 
                _config["ALtinn:EnterpriseUser:Password"], 
                _config["Certificate:Thumbprint"]);
            var reporteeHandler = new ReporteeHandler(_config["Altinn:ApiKey"]);
            var authorizationHandler = new AuthorizationHandler(_config["Altinn:ApiKey"]);

            Person testperson = TT02.GetTestPersons()[0];
            Service testservice = TT02.GetServiceCodes()[0];

            var maskinportenToken = await authenticationHandler.PostMaskinportenToken();
            var altinnExchangeToken = await authenticationHandler.GetAltinnExchangeToken(maskinportenToken.AccessToken);

            var reportee = await reporteeHandler.GetReportee(altinnExchangeToken, testperson.SSN, testperson.LastName);

            var delegations = await authorizationHandler.GetDelegations(altinnExchangeToken, reportee.ReporteeId);

            // Find any existing rights for the test service
            List<int> rightsToRemove = 
                (from r in delegations.Rights.Embedded.Rights
                where r.ServiceCode == testservice.ServiceCode && r.ServiceEditionCode == testservice.ServiceEditionCode
                select r.RightID)
                .ToList<int>();

            // By removing the delegations before posting new the POST will not fail since they already exist.
            foreach (int rightId in rightsToRemove)
            {
                await authorizationHandler.DeleteDelegatedRights(altinnExchangeToken, reportee.ReporteeId, rightId);
            }

            await authorizationHandler.PostDelegations(altinnExchangeToken, testperson.SSN, testperson.LastName, testservice);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
