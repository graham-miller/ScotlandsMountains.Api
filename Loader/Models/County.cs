namespace ScotlandsMountains.Api.Loader.Models
{
    public class County : Entity
    {
        public County() : base(PartitionKeyFrom.Type) { }

        public string Name { get; set; }
    }

    public class CountySummary : Summary
    {
        public CountySummary(County county)
            : base(county)
        {
            Name = county.Name;
        }

        public string Name { get; set; }
    }
}
