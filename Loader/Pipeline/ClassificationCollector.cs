using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScotlandsMountains.Api.Loader.Models;

namespace ScotlandsMountains.Api.Loader.Pipeline
{
    public class ClassificationCollector : ICollector<Classification>
    {
        public void CollectFrom(CollectorContext context)
        {
            var keys = context.Raw
                .Where(x => _nameLookup.ContainsKey(x.Key) && x.Value == "1")
                .Select(x => x.Key);

            foreach (var key in keys)
            {
                if (!_nameLookup.ContainsKey(key)) continue;

                if (!_items.TryGetValue(key, out var classification))
                {
                    classification = new Classification
                    {
                        Id = Guid.NewGuid().ToString("D"),
                        Name = _nameLookup[key]
                    };
                    _items.Add(key, classification);
                }

                classification.Mountains.Add(new MountainSummary(context.Mountain));
                context.Mountain.Classifications.Add(new ClassificationSummary(classification));
            }
        }

        public IList<Classification> Items => _items.Select(x => x.Value).ToList();

        private readonly IDictionary<string, Classification> _items = new Dictionary<string, Classification>();

        private readonly IDictionary<string, string> _nameLookup = new Dictionary<string, string>
        {
            {"M", "Munros"},
            {"MT", "Munro tops"},
            {"C", "Corbetts"},
            {"CT", "Corbett tops"},
            {"G", "Grahams"},
            {"GT", "Graham tops"},
            {"D", "Donalds"},
            {"DT", "Donald tops"},
            {"Ma", "Marilyns"},
            {"Mur", "Murdos"},
            {"Hu"  , "HuMPs"},
            {"4"   , "TuMPs (400 to 499m)"},
            {"3"   , "TuMPs (300 to 399m)"},
            {"2"   , "TuMPs (200 to 299m)"},
            {"1"   , "TuMPs (100 to 199m)"},
            {"0"   , "TuMPs (0 to 99m)"},
        };
    }
}