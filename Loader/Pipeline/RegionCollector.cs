using System;
using System.Collections.Generic;
using System.Linq;
using ScotlandsMountains.Api.Loader.Models;

namespace ScotlandsMountains.Api.Loader.Pipeline
{
    public class RegionCollector : ICollector<Region>
    {
        public void CollectFrom(CollectorContext context)
        {
            var key = context.Raw["Region"];

            if (!_items.TryGetValue(key, out var region))
            {
                var split = key.Split(':', StringSplitOptions.RemoveEmptyEntries);
                var code = split[0].Trim();
                var name = split[1].Trim();
                region = new Region
                {
                    Code = code,
                    Name = name
                };
                _items.Add(key, region);
            }

            context.Mountain.Region = new RegionSummary(region);
        }

        public IList<Region> Items => _items.Select(x => x.Value).ToList();

        private readonly IDictionary<string, Region> _items = new Dictionary<string, Region>();
    }
}