namespace ScotlandsMountains.Api.Loader.Models
{
    public class County
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class CountySummary
    {
        public CountySummary() { }

        public CountySummary(County county)
        {
            Id = county.Id;
            Name = county.Name;
        }

        public string Id { get; set; }
        
        public string Name { get; set; }
    }
}
