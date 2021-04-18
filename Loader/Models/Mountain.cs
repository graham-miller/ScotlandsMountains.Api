using System.Collections.Generic;

namespace ScotlandsMountains.Api.Loader.Models
{
    public class Mountain
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public Location Location { get; set; }
        
        public int DobihId { get; set; }
        
        public string GridRef { get; set; }
        
        public double Height { get; set; }
        
        public Prominence Prominence { get; set; }
        
        public string Features { get; set; }
        
        public string Observations { get; set; }
        
        public RegionSummary Region { get; set; }
        
        public CountySummary County { get; set; }

        public IList<ClassificationSummary> Classifications { get; set; } = new List<ClassificationSummary>();
    }
}
