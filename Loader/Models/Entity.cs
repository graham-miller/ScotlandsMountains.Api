using System;
using Humanizer;

namespace ScotlandsMountains.Api.Loader.Models
{
    public enum PartitionKeyFrom
    {
        Type,
        Id
    }

    public abstract class Entity
    {
        protected Entity(PartitionKeyFrom partitionKeyType)
        {
            Id = Guid.NewGuid().ToString("D");

            if (partitionKeyType == PartitionKeyFrom.Id)
                PartitionKey = Id;

            if (partitionKeyType == PartitionKeyFrom.Type)
                PartitionKey = GetType().Name.Camelize();
        }

        public string Id { get; }

        public string PartitionKey { get; }
    }
}
