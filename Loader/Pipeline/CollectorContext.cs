using System;
using System.Collections.Generic;
using ScotlandsMountains.Api.Loader.Models;

namespace ScotlandsMountains.Api.Loader.Pipeline
{
    public class CollectorContext
    {
        public CollectorContext(IDictionary<string, string> raw)
        {
            Raw = raw;
            Mountain = new Mountain();
        }

        public IDictionary<string, string> Raw { get; }

        public Mountain Mountain { get; }
    }
}