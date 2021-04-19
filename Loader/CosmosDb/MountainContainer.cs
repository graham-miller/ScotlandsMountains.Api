using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using ScotlandsMountains.Api.Loader.Models;

namespace ScotlandsMountains.Api.Loader.CosmosDb
{
    public class MountainContainer
    {
        private const string Id = "mountains";
        private const string PartitionKeyPath = "/id";

        private Database _database;
        private Container _container;

        public async Task Create()
        {
            _database = Shared.Client.GetDatabase("scotlands-mountains");

            var existingContainer = _database.GetContainer(Id);
            try
            {
                await existingContainer.DeleteContainerAsync();
            }
            catch (CosmosException exception) when (exception.StatusCode == HttpStatusCode.NotFound) { }


            var properties = new ContainerProperties
            {
                Id = Id,
                PartitionKeyPath = PartitionKeyPath,
                GeospatialConfig = new GeospatialConfig(GeospatialType.Geography)
            };
            properties.IndexingPolicy.IncludedPaths.Add(new IncludedPath {Path = "/*"});

            var locationPath = new SpatialPath {Path = "/location/?"};
            locationPath.SpatialTypes.Clear();
            locationPath.SpatialTypes.Add(SpatialType.Point);
            properties.IndexingPolicy.SpatialIndexes.Add(locationPath);

            await _database.CreateContainerAsync(properties);
            _container = _database.GetContainer(Id);
        }

        public async Task Load(IEnumerable<Mountain> mountains)
        {
            var cost = 0d;
            var inserted = 0;
            var errors = 0;

            foreach (var mountain in mountains)
            {
                var result = await Insert(mountain);
                var attempts = 1;

                while (result.StatusCode == HttpStatusCode.TooManyRequests && attempts < 5)
                {
                    attempts++;
                    await Task.Delay(500);
                    result = await Insert(mountain);
                }
                
                if (result.StatusCode == HttpStatusCode.Created)
                {
                    inserted++;
                    cost += result.RequestCharge;
                }
                else
                {
                    errors++;
                }
            }

            Console.WriteLine($"{inserted:#,##0} records inserted ({errors:#,##0} errors) at a cost of {cost:#.00} RUs");
        }

        private async Task<ItemResponse<Mountain>> Insert(Mountain mountain)
        {
            return await _container.CreateItemAsync(mountain, new PartitionKey(mountain.Id));
        }
    }
}
