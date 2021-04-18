namespace ScotlandsMountains.Api.Loader.Models
{
    public class Classification
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ClassificationSummary
    {
        public ClassificationSummary(Classification classification)
        {
            Id = classification.Id;
            Name = classification.Name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}