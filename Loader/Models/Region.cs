using System;

namespace ScotlandsMountains.Api.Loader.Models
{
    public class Region
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
    }

    public class RegionSummary
    {
        public RegionSummary(Region region)
        {
            Id = region.Id;
            Code = region.Code;
            Name = region.Name;
        }

        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
    }
}
