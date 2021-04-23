using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

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

        public async Task<IActionResult> GetClassifications()
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

        public async Task<IActionResult> GetClassification(string id)
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

            return await QueryMountainAspects(query);
        }

        public async Task<IActionResult> Search(string term)
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

        public async Task<IActionResult> GetMountain(string id)
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

            return await QueryMountains(query);
        }

        private async Task<IActionResult> QueryMountainAspects(string query)
        {
            return await QueryCosmosDb(query, AspectsContainerId);
        }

        private async Task<IActionResult> QueryMountains(string query)
        {
            return await QueryCosmosDb(query, MountainsContainerId);
        }

        private async Task<IActionResult> QueryCosmosDb(string query, string containerId)
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
                    var result = JsonConvert.DeserializeObject<dynamic>(await streamReader.ReadToEndAsync());
                    return new JsonResult(result.Documents);
                }
            }
        }
    }
}
