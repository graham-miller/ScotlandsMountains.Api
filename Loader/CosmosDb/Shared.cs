using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace ScotlandsMountains.Api.Loader.CosmosDb
{
    public static class Shared
    {
        static Shared()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config["CosmosDbConnectionString"];
            var clientOptions = new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            };
            Client = new CosmosClient(connectionString, clientOptions);
        }

        public static CosmosClient Client { get; set; }
    }
}
