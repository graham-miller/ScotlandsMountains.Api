using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ScotlandsMountains.Api.Functions
{
    public class Endpoints
    {
        private const string DatabaseId = "scotlands-mountains";

        private readonly CosmosClient _client;

        public Endpoints(CosmosClient client)
        {
            _client = client;
        }

        // http://localhost:7071/api/classifications
        [FunctionName("GetClassifications")]
        public async Task<IActionResult> GetClassifications(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "classifications")] HttpRequest req,
            ILogger log)
        {
            var query = @"
                SELECT
                    c.id,
                    c.name,
                    c.description
                FROM c
                WHERE c.partitionKey = 'classification'
                ORDER BY c.displayOrder ASC";

            return await QueryMountainAspects(query);
        }

        // http://localhost:7071/api/classifications/6a69287b-471a-4e72-92b3-55bfb599c3db
        [FunctionName("GetClassification")]
        public async Task<IActionResult> GetClassification(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "classifications/{id}")] HttpRequest req,
            string id,
            ILogger log)
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

        // http://localhost:7071/api/search?term=nev
        [FunctionName("Search")]
        public async Task<IActionResult> Search(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "search")] HttpRequest req,
            ILogger log)
        {
            if (!req.Query.TryGetValue("term", out var terms)) return new BadRequestResult();

            if (terms.Count != 1) return new BadRequestResult();

            var term = terms.Single();

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

        // http://localhost:7071/api/mountains/ddbf11aa-5fe1-42e9-8886-0518afc1c293
        [FunctionName("GetMountain")]
        public async Task<IActionResult> GetMountain(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "mountains/{id}")] HttpRequest req,
            string id,
            ILogger log)
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
            return await QueryCosmosDb(query, "mountainAspects");
        }

        private async Task<IActionResult> QueryMountains(string query)
        {
            return await QueryCosmosDb(query, "mountains");
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
