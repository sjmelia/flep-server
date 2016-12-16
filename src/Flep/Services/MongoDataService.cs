namespace Flep
{
    using System.Collections.Generic;
    using System.Linq;
    using Flep.Models;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.GeoJsonObjectModel;

    public class MongoDataService : IDataService
    {
        private const string CollectionName = "Messages";

        private static readonly int MetresPerMile = 8046;

        private MongoClient client;

        private IMongoDatabase db;

        public MongoDataService()
        {
            this.client = new MongoClient("mongodb://localhost:27017");
            this.db = this.client.GetDatabase("Messages");
            if (!this.CollectionExists(CollectionName))
            {
                this.db.CreateCollection(CollectionName);
                var keys = Builders<Message>
                    .IndexKeys
                    .Geo2DSphere(x => x.Location);
                this.db.GetCollection<Message>(CollectionName)
                    .Indexes
                    .CreateOne(keys);
            }
        }

        public IEnumerable<Message> Get(Location point)
        {
            var geographic = GeoJson.Geographic(
                    point.coordinates[0],
                    point.coordinates[1]);
            var geoJson = GeoJson.Point(geographic);
            var filter = Builders<Message>
                .Filter
                .Near(
                    x => x.Location,
                    geoJson,
                    MetresPerMile,
                    null);

            return this.db.GetCollection<Message>(CollectionName)
                .Find(filter)
                .SortByDescending(x => x.DateSubmitted)
                .ToList();
        }

        public void Add(Message msg)
        {
            this.db.GetCollection<Message>(CollectionName).InsertOne(msg);
        }

        private bool CollectionExists(string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var collections = this.db.ListCollections(
                    new ListCollectionsOptions { Filter = filter });
            return collections.ToList().Any();
        }
    }
}
