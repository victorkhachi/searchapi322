using MongoDB.Bson;
using MongoDB.Driver;

namespace SearchAPI{
    public class DBrepository{
        private MongoClient _mongoClient;
        private IMongoDatabase _mongoDatabase;

        public DBrepository(string connectionString, string databaseName)
        {
            _mongoClient = new MongoClient(connectionString);
            _mongoDatabase = _mongoClient.GetDatabase(databaseName);
        }

        public void InsertDocument<T>(string collectionName, T document)
        {
            var collection = _mongoDatabase.GetCollection<T>(collectionName);
            collection.InsertOne(document);
        }

        public List<T> GetDocuments<T>(string collectionName)
        {
            var collection = _mongoDatabase.GetCollection<T>(collectionName);
            return collection.Find(new BsonDocument()).ToList();
        }

        public T GetDocumentById<T>(string collectionName, int documentId)
        {
            var collection = _mongoDatabase.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", documentId);
            return collection.Find(filter).FirstOrDefault();
        }


        // Implement other methods as needed for updating and deleting documents
        public void UpdateDocument<T>(string collectionName, string documentId, T updatedDocument)
        {
            var collection = _mongoDatabase.GetCollection<T>(collectionName);

            var filter = Builders<T>.Filter.Eq("_id", documentId); // Assuming "_id" is the field representing the document's unique identifier
            var update = Builders<T>.Update.Set(x => x, updatedDocument);

            collection.UpdateOne(filter, update);
        }
        public void DeleteDocument<T>(string collectionName, string documentId)
        {
            var collection = _mongoDatabase.GetCollection<T>(collectionName);

            var filter = Builders<T>.Filter.Eq("_id", documentId); // Assuming "_id" is the field representing the document's unique identifier

            collection.DeleteOne(filter);
        }
    }
}