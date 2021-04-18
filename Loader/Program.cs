﻿using System.Collections.Generic;
using System.Linq;
using ScotlandsMountains.Api.Loader.Models;
using ScotlandsMountains.Api.Loader.Pipeline;

namespace ScotlandsMountains.Api.Loader
{
    class Program
    {
        private static readonly string[] IncludedCountries = {"S", "ES"}; 

        static void Main(string[] args)
        {
            var collector = new CollectorPipeline();

            using var csv = new HillCsvReader();
            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                var record = ReadRecordFrom(csv);

                if (IsScottish(record))
                    collector.CollectFrom(new CollectorContext(record));
            }

            var mountains = collector.Of<Mountain>().Items;
            var regions = collector.Of<Region>().Items;
            var counties = collector.Of<County>().Items;
            var classifications = collector.Of<Classification>().Items;
            var maps = collector.Of<Map>().Items;

            var mountain = mountains.Single(x => x.Name.Contains("Foinaven"));

            //var container = new MountainContainer();
            //await container.Create();
            //await container.Load(mountains.Take(100));
        }

        private static Dictionary<string, string> ReadRecordFrom(HillCsvReader csv)
        {
            return new Dictionary<string, string>(
                ((IDictionary<string, object>)csv.GetRecord<dynamic>())
                .Select(y => new KeyValuePair<string, string>(y.Key, y.Value.ToString())));
        }

        private static bool IsScottish(Dictionary<string, string> record)
        {
            return IncludedCountries.Contains(record["Country"]);
        }
    }
}