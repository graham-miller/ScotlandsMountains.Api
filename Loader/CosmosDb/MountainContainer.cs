using Microsoft.Azure.Cosmos;

namespace ScotlandsMountains.Api.Loader.CosmosDb
{
    public class MountainContainer : GenericContainer
    {
        public MountainContainer() : base("mountains") { }

        protected override ContainerProperties GetContainerProperties()
        {
            var properties = new ContainerProperties
            {
                Id = ContainerId,
                PartitionKeyPath = PartitionKeyPath,
                GeospatialConfig = new GeospatialConfig(GeospatialType.Geography)
            };
            properties.IndexingPolicy.IncludedPaths.Add(new IncludedPath {Path = "/*"});

            var locationPath = new SpatialPath {Path = "/location/?"};
            locationPath.SpatialTypes.Clear();
            locationPath.SpatialTypes.Add(SpatialType.Point);
            properties.IndexingPolicy.SpatialIndexes.Add(locationPath);

            return properties;
        }
    }
}
