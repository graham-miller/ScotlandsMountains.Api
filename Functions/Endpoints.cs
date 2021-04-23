using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace ScotlandsMountains.Api.Functions
{
    public class Endpoints
    {
        private readonly MountainsRepository _mountainsRepository;

        public Endpoints(MountainsRepository mountainsRepository)
        {
            _mountainsRepository = mountainsRepository;
        }

        // http://localhost:7071/api/classifications
        [FunctionName("GetClassifications")]
        public async Task<IActionResult> GetClassifications(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "classifications")] HttpRequest request,
            ILogger logger)
        {
            return await _mountainsRepository.GetClassifications();
        }

        // http://localhost:7071/api/classifications/6a69287b-471a-4e72-92b3-55bfb599c3db
        [FunctionName("GetClassification")]
        public async Task<IActionResult> GetClassification(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "classifications/{id}")] HttpRequest request,
            string id,
            ILogger logger)
        {
            return await _mountainsRepository.GetClassification(id);
        }

        // http://localhost:7071/api/search?term=nev
        [FunctionName("Search")]
        public async Task<IActionResult> Search(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "search")] HttpRequest request,
            ILogger logger)
        {
            if (!request.Query.TryGetValue("term", out var terms)) return new BadRequestResult();

            if (terms.Count != 1) return new BadRequestResult();

            var term = terms.Single();

            return await _mountainsRepository.Search(term);
        }

        // http://localhost:7071/api/mountains/ddbf11aa-5fe1-42e9-8886-0518afc1c293
        [FunctionName("GetMountain")]
        public async Task<IActionResult> GetMountain(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "mountains/{id}")] HttpRequest request,
            string id,
            ILogger logger)
        {
            return await _mountainsRepository.GetMountain(id);
        }
    }
}
