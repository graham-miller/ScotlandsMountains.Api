using System.Collections.Generic;
using System.Linq;

namespace ScotlandsMountains.Api.Loader.Pipeline
{
    public class CollectorPipeline
    {
        private readonly IList<ICollector> _collectors = new List<ICollector>
        {
            new MountainCollector(),
            new RegionCollector(),
            new CountyCollector(),
            new ClassificationCollector(),
            new MapCollector()
        };

        public void CollectFrom(CollectorContext context)
        {
            foreach (var collector in _collectors)
                collector.CollectFrom(context);
        }

        public ICollector<T> Of<T>() => _collectors.OfType<ICollector<T>>().Single();
    }
}
