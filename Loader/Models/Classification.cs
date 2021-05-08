using System.Collections.Generic;

namespace ScotlandsMountains.Api.Loader.Models
{
    public class Classification : Entity
    {
        public Classification() : base(PartitionKeyFrom.Type) { }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public string Description { get; set; }

        public IList<NumberedMountainSummary> Mountains { get; set; } = new List<NumberedMountainSummary>();
    }

    public class ClassificationSummary : Summary
    {
        public ClassificationSummary(Classification classification)
            : base(classification)
        {
            Name = classification.Name;
            DisplayOrder = classification.DisplayOrder;
        }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }
    }

    public class NumberedMountainSummary : MountainSummary
    {
        public NumberedMountainSummary(Mountain mountain)
            : base(mountain)
        { }

        public int Position { get; set; }
    }
}