using System.Collections.Generic;

namespace ScotlandsMountains.Api.Loader.Models
{
    public class Classification : Entity
    {
        public Classification() : base(PartitionKeyFrom.Type) { }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public string Description { get; set; }

        public IList<MountainSummary> Mountains { get; set; } = new List<MountainSummary>();
    }

    public class ClassificationSummary : Summary
    {
        public ClassificationSummary(Classification classification)
            : base(classification)
        {
            Name = classification.Name;
        }

        public string Name { get; set; }
    }
}