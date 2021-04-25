using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ScotlandsMountains.Api.Functions
{
    public class Endpoints
    {
        private readonly MountainsRepository _mountainsRepository;

        public Endpoints(MountainsRepository mountainsRepository)
        {
            _mountainsRepository = mountainsRepository;
        }

        // http://localhost:7071/api/initial
        [FunctionName("GetInitialData")]
        public async Task<IActionResult> GetInitialData(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "initial")] HttpRequest request,
            ILogger logger)
        {
            var classifications = await _mountainsRepository.GetClassifications();
            var id = classifications.First()["id"].ToString();
            var mountains = await _mountainsRepository.GetClassification(id);
            var result = new JObject(
                new JProperty("classifications", classifications),
                new JProperty("mountains", mountains));
            
            return new OkObjectResult(result);
        }

        // http://localhost:7071/api/classifications
        [FunctionName("GetClassifications")]
        public async Task<IActionResult> GetClassifications(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "classifications")] HttpRequest request,
            ILogger logger)
        {
            var result = await _mountainsRepository.GetClassifications();
            return new OkObjectResult(result);
        }

        // http://localhost:7071/api/classifications/6a69287b-471a-4e72-92b3-55bfb599c3db
        [FunctionName("GetClassification")]
        public async Task<IActionResult> GetClassification(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "classifications/{id}")] HttpRequest request,
            string id,
            ILogger logger)
        {
            var result = await _mountainsRepository.GetClassification(id);

            if (result == null) return new NotFoundResult();

            return new OkObjectResult(result);
        }

        // http://localhost:7071/api/mountains/ddbf11aa-5fe1-42e9-8886-0518afc1c293

        [FunctionName("GetMountain")]
        public async Task<IActionResult> GetMountain(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "mountains/{id}")] HttpRequest request,
            string id,
            ILogger logger)
        {
            var result = await _mountainsRepository.GetMountain(id);

            if (result == null) return new NotFoundResult();

            return new OkObjectResult(result);
        }

        // http://localhost:7071/api/search?term=nev
        [FunctionName("Search")]
        public async Task<IActionResult> Search(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", "POST", Route = "search")] HttpRequest request,
            ILogger logger)
        {
            var term = request.Query.GetString("term");
            if (string.IsNullOrWhiteSpace(term) || term.Length < 3) return new BadRequestResult();

            var pageSize = request.Query.GetInt("pageSize") ?? 10;

            string continuationToken  = null;
            if (request.Method == HttpMethods.Post)
            {
                var json = await new StreamReader(request.Body).ReadToEndAsync();
                continuationToken = JsonConvert.DeserializeObject<dynamic>(json).continuationToken;
            }

            var result = await _mountainsRepository.Search(term, pageSize, continuationToken);
            return new OkObjectResult(result);
        }
    }
}
