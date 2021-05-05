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
                    classification = _nameLookup[key];
                    _items.Add(key, classification);
                }

                classification.Mountains.Add(new NumberedMountainSummary(context.Mountain));
                context.Mountain.Classifications.Add(new ClassificationSummary(classification));
            }
        }

        public IList<Classification> Items => GetFinalizedList();

        private List<Classification> GetFinalizedList()
        {
            var classifications = _items.Select(x => x.Value).ToList();

            foreach (var classification in classifications)
            {
                classification.Description = string.Format(classification.Description, classification.Mountains.Count);
                classification.Mountains = classification.Mountains
                    .OrderByDescending(x => x.Height.Metres)
                    .Select((mountain, index) =>
                    {
                        mountain.Position = index + 1;
                        return mountain;
                    })
                    .ToList();
            }

            return classifications;
        }

        private readonly IDictionary<string, Classification> _items = new Dictionary<string, Classification>();

        private readonly IDictionary<string, Classification> _nameLookup = new Dictionary<string, Classification>
        {
            {"M", new Classification
            {
                Name = "Munros",
                DisplayOrder = 1,
                Description = "The Munros are the most significant mountains in Scotland over 3,000 feet (914.4m), according to original compiler Sir Hugh Munro. The list was first drawn up in 1891, and is modified from time to time by the Scottish Mountaineering Club (SMC). The list contains {0:#,##0} mountains."
            }},
            {"MT", new Classification
            {
                Name = "Munro tops",
                DisplayOrder = 2,
                Description = "Munro tops are lesser peaks over 3,000 feet but which are lower than the nearby primary mountain. The list is maintained by the Scottish Mountaineering Club (SMC). The list contains {0:#,##0} tops."
            }},
            {"C", new Classification
            {
                Name = "Corbetts",
                DisplayOrder = 3,
                Description = "The Corbetts are mountains in Scotland between 2,500 and 3,000 feet (762 and 914.4 metres), with a prominence of at least 500 feet (152.4 metres). The list is maintained by the Scottish Mountaineering Club (SMC). The list contains {0:#,##0} mountains."
            }},
            {"CT", new Classification
            {
                Name = "Corbett tops",
                DisplayOrder = 4,
                Description = "Corbett tops are lesser peaks between 2,500 and 3,000 feet (762 and 914.4 metres), with a prominence of at least 500 feet (152.4 metres) but which are lower than the nearby primary mountain. The list is maintained by the Scottish Mountaineering Club (SMC). The list contains {0:#,##0} tops."
            }},
            {"G", new Classification
            {
                Name = "Grahams",
                DisplayOrder = 5,
                Description = "The Grahams are hills in Scotland between 2,000 and 2,500 feet (609.6 and 762 metres), with a prominence of at least 150 metres (492 feet). The list is maintained by the Scottish Mountaineering Club (SMC). The list contains {0:#,##0} hills."
            }},
            {"GT", new Classification
            {
                Name = "Graham tops",
                DisplayOrder = 6,
                Description = "Graham tops are lesser hills between 2,000 and 2,500 feet (609.6 and 762 metres), with a prominence of at least 150 metres (492 feet) but which are lower than the nearby primary hill. The list contains {0:#,##0} tops."
            }},
            {"D", new Classification
            {
                Name = "Donalds",
                DisplayOrder = 7,
                Description = "The Donalds are hills in the Scottish Lowlands over 2,000 feet (609.6 metres). The list is maintained by the Scottish Mountaineering Club (SMC). The list contains {0:#,##0} hills."
            }},
            {"DT", new Classification
            {
                Name = "Donald tops",
                DisplayOrder = 8,
                Description = "Donald tops are lesser hills in the Scottish Lowlands over 2,000 feet (609.6 metres), with a prominence of at least 150 metres (492 feet) but which are lower than the nearby primary hill. The list contains {0:#,##0} tops."
            }},
            {"Ma", new Classification
            {
                Name = "Marilyns",
                DisplayOrder = 9,
                Description = "The Marilyns are hills in the British Isles that have a prominence of at least 150 metres, regardless of distance, absolute height or merit. There are {0:#,##0} Scottish Marilyns."
            }},
            {"Mur", new Classification
            {
                Name = "Murdos",
                DisplayOrder = 10,
                Description = "The Murdos are Scottish mountains over 3,000 feet (914.4 metres), with a prominence of at least 30 metres (98 feet). The list contains {0:#,##0} mountains."
            }},
            {"Hu", new Classification
            {
                Name = "HuMPs",
                DisplayOrder = 11,
                Description = "The HuMPs (Hundred and upwards Metre Prominence) are hills in the British Isles that have a prominence of at least 100 metres, regardless of distance, absolute height or merit. There are {0:#,##0} Scottish HuMPs."
            }},
            {"4", new Classification
            {
                Name = "TuMPs (400 to 499m)",
                DisplayOrder = 12,
                Description = "The TuMPs (Thirty and upwards Metre Prominence) are hills in the British Isles that have a prominence of at least 30 metres, regardless of distance, absolute height or merit. There are {0:#,##0} Scottish TuMPS (400 to 499m)."
            }},
            {"3", new Classification
            {
                Name = "TuMPs (300 to 399m)",
                DisplayOrder = 13,
                Description = "The TuMPs (Thirty and upwards Metre Prominence) are hills in the British Isles that have a prominence of at least 30 metres, regardless of distance, absolute height or merit. There are {0:#,##0} Scottish TuMPS (300 to 399m)."
            }},
            {"2", new Classification
            {
                Name = "TuMPs (200 to 299m)",
                DisplayOrder = 14,
                Description = "The TuMPs (Thirty and upwards Metre Prominence) are hills in the British Isles that have a prominence of at least 30 metres, regardless of distance, absolute height or merit. There are {0:#,##0} Scottish TuMPS (200 to 299m)."
            }},
            {"1", new Classification
            {
                Name = "TuMPs (100 to 199m)",
                DisplayOrder = 15,
                Description = "The TuMPs (Thirty and upwards Metre Prominence) are hills in the British Isles that have a prominence of at least 30 metres, regardless of distance, absolute height or merit. There are {0:#,##0} Scottish TuMPS (100 to 199m)."
            }},
            {"0", new Classification
            {
                Name = "TuMPs (0 to 99m)",
                DisplayOrder = 16,
                Description = "The TuMPs (Thirty and upwards Metre Prominence) are hills in the British Isles that have a prominence of at least 30 metres, regardless of distance, absolute height or merit. There are {0:#,##0} Scottish TuMPS (0 to 99m)."
            }}
        };

    }
}