namespace ScotlandsMountains.Api.Loader.Models
{
    public class Map : Entity
    {
        public Map() : base(PartitionKeyFrom.Type) { }

        public string Code { get; set; }
        
        public double Scale { get; set; }
    }

    public class MapSummary : Summary
    {
        public MapSummary(Map map)
            : base(map)
        {
            Code = map.Code;
            Scale = map.Scale;
        }

        public string Code { get; set; }
        
        public double Scale { get; set; }
    }
}
