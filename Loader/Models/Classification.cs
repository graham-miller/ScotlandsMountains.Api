using System.Collections.Generic;

namespace ScotlandsMountains.Api.Loader.Models
{
    public class Classification
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IList<MountainSummary> Mountains { get; set; } = new List<MountainSummary>();
    }

    public class ClassificationSummary
    {
        public ClassificationSummary() { }

        public ClassificationSummary(Classification classification)
        {
            Id = classification.Id;
            Name = classification.Name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}