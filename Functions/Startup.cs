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
            builder.Services.AddSingleton<CosmosClient>((s) =>
            {
                var connectionString = System.Environment.GetEnvironmentVariable("CosmosDbConnectionString");
                //var clientOptions = new CosmosClientOptions
                //{
                //    SerializerOptions = new CosmosSerializationOptions
                //    {
                //        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                //    }
                //};
                return new CosmosClient(connectionString);//, clientOptions);
            });
        }
    }
}