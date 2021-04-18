using System;
using System.Collections.Generic;
using System.Linq;
using ScotlandsMountains.Api.Loader.Models;

namespace ScotlandsMountains.Api.Loader.Pipeline
{
    public class CountyCollector : ICollector<County>
    {
        private readonly IDictionary<string, County> _items = new Dictionary<string, County>();

        public void CollectFrom(CollectorContext context)
        {
            var key = context.Raw["County"];

            if (!_items.TryGetValue(key, out var county))
            {
                county = new County
                {
                    Id = Guid.NewGuid().ToString("D"),
                    Name = context.Raw["County"]
                };
                _items.Add(key, county);
            }

            context.Mountain.County = new CountySummary(county);
        }

        public IEnumerable<County> Items => _items.Select(x => x.Value);
    }
}