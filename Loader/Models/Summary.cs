namespace ScotlandsMountains.Api.Loader.Models
{
    public abstract class Summary
    {
        protected Summary(Entity entity)
        {
            Id = entity.Id;
            PartitionKey = entity.PartitionKey;
        }

        public string Id { get; }

        public string PartitionKey { get; }
    }
}