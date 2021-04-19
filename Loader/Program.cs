﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ScotlandsMountains.Api.Loader.CosmosDb;
using ScotlandsMountains.Api.Loader.Models;
using ScotlandsMountains.Api.Loader.Pipeline;
using ScotlandsMountains.Api.Loader.Resources;

namespace ScotlandsMountains.Api.Loader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var collector = new CollectorPipeline();
            ReadHillCsv(collector);

            var mountains = collector.Of<Mountain>().Items;
            var classifications = collector.Of<Classification>().Items;
            var maps = collector.Of<Map>().Items;
            var regions = collector.Of<Region>().Items;
            var counties = collector.Of<County>().Items;

            var mountainContainer = new MountainContainer();
            await mountainContainer.Create();
            await mountainContainer.Save(mountains.Take(100));

            var aspectsContainer = new GenericContainer("mountainAspects");
            await aspectsContainer.Create();
            await aspectsContainer.Save(classifications);
            await aspectsContainer.Save(maps);
            await aspectsContainer.Save(regions);
            await aspectsContainer.Save(counties);

            stopwatch.Stop();

            Console.WriteLine($"Time taken: {(stopwatch.ElapsedMilliseconds/1000):0s}");
        }

        private static void ReadHillCsv(CollectorPipeline collector)
        {
            using var csv = new HillCsvReader();
            csv.Read();
            csv.ReadHeader();

            csv.GetRecords<dynamic>()
                .Select(RecordAsDictionary)
                .OrderByDescending(Height)
                .ToList()
                .ForEach(x =>
                {
                    if (IsScottish(x))
                        collector.CollectFrom(new CollectorContext(x));
                });
        }

        private static IDictionary<string, string> RecordAsDictionary(dynamic record)
        {
            var keyValuePairs =
                ((IDictionary<string, object>) record).Select(y =>
                    new KeyValuePair<string, string>(y.Key, y.Value.ToString()));

            return new Dictionary<string, string>(keyValuePairs);
        }

        private static double Height(IDictionary<string, string> x)
        {
            return double.Parse(x["Metres"]);
        }

        private static bool IsScottish(IDictionary<string, string> record)
        {
            return IncludedCountries.Contains(record["Country"]);
        }

        private static readonly string[] IncludedCountries = {"S", "ES"};
    }
}