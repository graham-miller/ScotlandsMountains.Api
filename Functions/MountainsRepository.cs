using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ScotlandsMountains.Api.Functions
{
    public class MountainsRepository
    {
        private const string DatabaseId = "scotlands-mountains";
        private const string MountainsContainerId = "mountains";
        private const string AspectsContainerId = "mountainAspects";

        private readonly CosmosClient _client;

        public MountainsRepository(CosmosClient client)
        {
            _client = client;
        }

        public async Task<JArray> GetClassifications()
        {
            const string query = @"
                SELECT
                    c.id,
                    c.name,
                    c.description
                FROM c
                WHERE c.partitionKey = 'classification'
                ORDER BY c.displayOrder ASC";

            return await QueryMountainAspects(query);
        }

        public async Task<JToken> GetClassification(string id)
        {
            var query = $@"
                SELECT
                    c.id,
                    c.name,
                    c.description,
                    c.mountains
                FROM c
                WHERE c.partitionKey = 'classification'
                AND c.id = '{id}'
                ORDER BY c.displayOrder ASC";

            var result = await QueryMountainAspects(query);

            return result.Any() ? result.First() : null;
        }

        public async Task<JArray> Search(string term)
        {
            var query = $@"
                SELECT
                    '{term}' AS term,
                    c.name,
                    c.location,
                    c.height,
                    c.id,
                    c.partitionKey
                FROM c
                WHERE CONTAINS(c.name, '{term}', true)
                ORDER BY c.height.metres DESC";

            return await QueryMountains(query);
        }

        public async Task<JToken> GetMountain(string id)
        {
            var query = $@"
                SELECT
                    c.name,
                    c.aliases,
                    c.location,
                    c.gridRef,
                    c.height,
                    c.prominence,
                    c.features,
                    c.observations,
                    c.parent,
                    c.region,
                    c.county,
                    c.classifications,
                    c.maps,
                    c.id,
                    c.partitionKey
                FROM c
                WHERE c.partitionKey = '{id}'
                AND c.id = '{id}'";

            var result = await QueryMountains(query);

            return result.Any() ? result.First() : null;
        }

        private async Task<JArray> QueryMountainAspects(string query)
        {
            return await QueryCosmosDb(query, AspectsContainerId);
        }

        private async Task<JArray> QueryMountains(string query)
        {
            return await QueryCosmosDb(query, MountainsContainerId);
        }

        private async Task<JArray> QueryCosmosDb(string query, string containerId)
        {
            var container = _client.GetDatabase(DatabaseId).GetContainer(containerId);
            var iterator = container.GetItemQueryStreamIterator(query);

            using (var memoryStream = new MemoryStream())
            {
                while (iterator.HasMoreResults)
                {
                    using (var result = await iterator.ReadNextAsync())
                    {
                        await result.Content.CopyToAsync(memoryStream);
                    }
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                using (var streamReader = new StreamReader(memoryStream))
                {
                    var json = await streamReader.ReadToEndAsync();
                    var x = JsonConvert.DeserializeObject<JObject>(json) as IEnumerable<KeyValuePair<string, JToken>>;
                    var y = (JArray) x.First(j => j.Key == "Documents").Value;

                    return y;
                }
            }
        }
    }
}
