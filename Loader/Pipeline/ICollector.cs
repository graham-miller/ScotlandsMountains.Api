using System.Collections.Generic;

namespace ScotlandsMountains.Api.Loader.Pipeline
{
    public interface ICollector
    {
        void CollectFrom(CollectorContext context);
    }

    public interface ICollector<T> : ICollector
    {
        IList<T> Items { get; }
    }
}