using System;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ScotlandsMountains.Api.Functions;

[assembly: FunctionsStartup(typeof(Startup))]

namespace ScotlandsMountains.Api.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(s =>
            {
                var connectionString = Environment.GetEnvironmentVariable("CosmosDbConnectionString");
                return new CosmosClient(connectionString);
            });

            builder.Services.AddScoped<MountainsRepository, MountainsRepository>();
        }
    }
}