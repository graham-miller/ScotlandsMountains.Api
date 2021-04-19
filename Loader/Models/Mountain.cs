using System.Collections.Generic;

namespace ScotlandsMountains.Api.Loader.Models
{
    public class Mountain : Entity
    {
        public Mountain() : base(PartitionKeyFrom.Id) { }

        public string Name { get; set; }

        public List<string> Aliases { get; set; }

        public Location Location { get; set; }
        
        public int DobihId { get; set; }
        
        public string GridRef { get; set; }
        
        public Height Height { get; set; }
        
        public Prominence Prominence { get; set; }
        
        public string Features { get; set; }
        
        public string Observations { get; set; }

        public MountainSummary Parent { get; set; }

        public RegionSummary Region { get; set; }

        public CountySummary County { get; set; }

        public IList<ClassificationSummary> Classifications { get; set; } = new List<ClassificationSummary>();

        public IList<MapSummary> Maps { get; set; } = new List<MapSummary>();
    }

    public class MountainSummary : Summary
    {
        public MountainSummary(Mountain mountain)
            : base(mountain)
        {
            Name = mountain.Name;
            Location = mountain.Location;
            Height = mountain.Height;
        }

        public string Name { get; set; }

        public Location Location { get; set; }

        public Height Height { get; set; }
    }
}
