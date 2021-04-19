namespace ScotlandsMountains.Api.Loader.Models
{
    public class Region : Entity
    {
        public Region() : base(PartitionKeyFrom.Type) { }

        public string Code { get; set; }

        public string Name { get; set; }
    }

    public class RegionSummary : Summary
    {
        public RegionSummary(Region region)
            : base(region)
        {
            Code = region.Code;
            Name = region.Name;
        }

        public string Code { get; set; }

        public string Name { get; set; }
    }
}
