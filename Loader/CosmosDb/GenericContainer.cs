using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.Azure.Cosmos;
using ScotlandsMountains.Api.Loader.Models;

namespace ScotlandsMountains.Api.Loader.CosmosDb
{
    public class GenericContainer
    {
        private const string DatabaseId = "scotlands-mountains";
        
        protected const string PartitionKeyPath = "/partitionKey";
        protected readonly string ContainerId;

        private Database _database;
        private Container _container;

        public GenericContainer(string containerId)
        {
            ContainerId = containerId;
        }

        public async Task Create()
        {
            _database = Shared.Client.GetDatabase(DatabaseId);

            var existingContainer = _database.GetContainer(ContainerId);
            try
            {
                await existingContainer.DeleteContainerAsync();
            }
            catch (CosmosException exception) when (exception.StatusCode == HttpStatusCode.NotFound) { }

            await _database.CreateContainerAsync(GetContainerProperties());
            _container = _database.GetContainer(ContainerId);
        }

        public async Task Save<T>(IEnumerable<T> items) where T : Entity
        {
            var count = items.Count();
            var cost = 0d;
            var inserted = 0;
            var errors = 0;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (var item in items)
            {
                var result = await Insert(item);
                var attempts = 1;

                while (result.StatusCode == HttpStatusCode.TooManyRequests && attempts < 5)
                {
                    attempts++;
                    await Task.Delay(100);
                    result = await Insert(item);
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

                if ((inserted + errors) % 100 == 0)
                {
                    Console.WriteLine($"Inserted {inserted:#,##0} of {count:#,##0} {typeof(T).Name.ToLower().Pluralize()} ({errors:#,##0} errors)");
                }
            }

            stopwatch.Stop();
            var seconds = stopwatch.ElapsedMilliseconds / 1000;
            Console.WriteLine($"Inserted {inserted:#,##0} {typeof(T).Name} records ({errors:#,##0} errors) in {seconds:#,##0}s at a cost of {cost:#,##0.00} RUs");
        }

        protected virtual ContainerProperties GetContainerProperties()
        {
            var properties = new ContainerProperties
            {
                Id = ContainerId,
                PartitionKeyPath = PartitionKeyPath
            };
            properties.IndexingPolicy.IncludedPaths.Add(new IncludedPath { Path = "/*" });

            return properties;
        }

        private async Task<ItemResponse<T>> Insert<T>(T item) where T : Entity
        {
            var itemRequestOptions = new ItemRequestOptions {EnableContentResponseOnWrite = false};
            var partitionKey = new PartitionKey(item.PartitionKey);

            return await _container.CreateItemAsync(item, partitionKey, itemRequestOptions);
        }
    }
}
