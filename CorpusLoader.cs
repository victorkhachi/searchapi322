using MongoDB.Bson;
using MongoDB.Driver;
using System.IO;

namespace SearchAPI
{
    public class CorpusLoader
    {
        private string corpusFilePath;
        private MongoClient mongoClient;
        private IMongoDatabase mongoDatabase;
        private IMongoCollection<BsonDocument> collection;

        public CorpusLoader(string corpusFilePath, string connectionString, string databaseName, string collectionName)
        {
            this.corpusFilePath = corpusFilePath;
            mongoClient = new MongoClient(connectionString);
            mongoDatabase = mongoClient.GetDatabase(databaseName);
            collection = mongoDatabase.GetCollection<BsonDocument>(collectionName);
        }

        public void LoadCorpus()
        {
            string[] lines = File.ReadAllLines(corpusFilePath);

            foreach (string line in lines)
            {
                var document = new BsonDocument
                {
                    { "title", "Document Title" },
                    { "content", line },
                    { "url", "Document URL" }
                };

                collection.InsertOne(document);
            }
        }
    }
}
