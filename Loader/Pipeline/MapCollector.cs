using System;
using System.Collections.Generic;
using System.Linq;
using ScotlandsMountains.Api.Loader.Models;

namespace ScotlandsMountains.Api.Loader.Pipeline
{
    public class MapCollector : ICollector<Map>
    {
        public void CollectFrom(CollectorContext context)
        {
            CollectMapsFrom(context, "Map 1:50k", 1d/50000);
            CollectMapsFrom(context, "Map 1:25k", 1d/25000);
        }

        private void CollectMapsFrom(CollectorContext context, string field, double scale)
        {
            foreach (var key in context.Raw[field].Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (!_items.TryGetValue(key, out var map))
                {
                    map = new Map
                    {
                        Code = key,
                        Scale = scale
                    };
                    _items.Add(key, map);
                }

                context.Mountain.Maps.Add(new MapSummary(map));
            }
        }

        public IList<Map> Items => _items.Select(x => x.Value).ToList();

        private readonly IDictionary<string, Map> _items = new Dictionary<string, Map>();
    }
}