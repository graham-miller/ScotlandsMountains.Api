using System.Collections.Generic;
using System.Linq;
using ScotlandsMountains.Api.Loader.Models;

namespace ScotlandsMountains.Api.Loader.Pipeline
{
    public class CountyCollector : ICollector<County>
    {
        public void CollectFrom(CollectorContext context)
        {
            var key = context.Raw["County"];

            if (!_items.TryGetValue(key, out var county))
            {
                county = new County
                {
                    Name = context.Raw["County"]
                };
                _items.Add(key, county);
            }

            context.Mountain.County = new CountySummary(county);
        }

        public IList<County> Items => _items.Select(x => x.Value).ToList();

        private readonly IDictionary<string, County> _items = new Dictionary<string, County>();
    }
}