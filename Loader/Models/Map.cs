namespace ScotlandsMountains.Api.Loader.Models
{
    public class Map
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public double Scale { get; set; }
    }

    public class MapSummary
    {
        public MapSummary() { }

        public MapSummary(Map map)
        {
            Id = map.Id;
            Code = map.Code;
            Scale = map.Scale;
        }

        public string Id { get; set; }
        public string Code { get; set; }
        public double Scale { get; set; }
    }
}
