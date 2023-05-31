using System.IO;
using System.Timers;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SearchAPI
{
    // Indexer class
    public class Indexer
    {
        private Dictionary<string, List<Posting>> invertedIndex;
        private Dictionary<string, Dictionary<string, int>> termFrequencies;
        private Dictionary<string, int> documentFrequencies;
        private DocumentParser documentParser;
        private DBrepository dbRepository;
        private System.Timers.Timer indexerTimer;

        public Indexer(DocumentParser documentParser,  DBrepository dbRepository)
        {
            invertedIndex = new Dictionary<string, List<Posting>>();
            this.documentParser = documentParser;
            termFrequencies = new Dictionary<string, Dictionary<string, int>>();
            documentFrequencies = new Dictionary<string, int>();
            this.dbRepository = dbRepository;
            indexerTimer = new System.Timers.Timer();
            indexerTimer.Interval = 2 * 60 * 60 * 1000; // 2 hours in milliseconds
            indexerTimer.Elapsed += IndexerTimerElapsed;
        }
        public void StartIndexer()
        {
            indexerTimer.Start();
        }

        public void StopIndexer()
        {
            indexerTimer.Stop();
        }

        private void IndexerTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            string folderPath = "Path to your corpus folder";
            LoadCorpus(folderPath);
        }

        // Method to load the corpus (all files in a folder) into MongoDB
        public void LoadCorpus(string folderPath)
        {
            int num = 0;
            string[] filePaths = Directory.GetFiles(folderPath);

            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string fileContent = File.ReadAllText(filePath);
                var document = new Document(num, fileName, fileContent, "Document URL");
                num++;
                AddDocumentToIndex(document);
            }
        }
   
         // Method to add a document to the index
        public void AddDocumentToIndex(Document document){
             // Store the document in the database
            dbRepository.InsertDocument("documents", document);
             // Parse the document to get the words and their frequency
            var wordsCount = documentParser.Parse();
            foreach(string word in wordsCount.Keys){
                if(!invertedIndex.ContainsKey(word)){
                    invertedIndex[word] = new List<Posting>();
                }
                List<Posting> postings = invertedIndex[word];
                Posting? posting = postings.Find(p => p.DocumentId == document.GetID());
                if (posting != null)
                {
                    posting.Frequency++;
                }
                else
                {
                    posting = new Posting(document.GetID(), 1);
                    postings.Add(posting);
                }
            }
        }
        //Method to search index and return set of documents that match terms query
        public HashSet<Document> SearchIndex(string query)
        {
            HashSet<Document> result = new HashSet<Document>();
            if (invertedIndex.ContainsKey(query))
            {
                List<Posting> postings = invertedIndex[query];
                foreach (Posting posting in postings)
                {
                    Document document = dbRepository.GetDocumentById<Document>("documents", posting.DocumentId);
                    if (document != null)
                    {
                        result.Add(document);
                    }
                }
            }
            return result;
        }


         // Method to get Term Frequency in a document
         public int GetTermFrequency(string term, int documentId)
        {
            string documentIdString = documentId.ToString(); 
            if (termFrequencies.ContainsKey(term) && termFrequencies[term].ContainsKey(documentIdString))
            {
                return termFrequencies[term][documentIdString];
            }
            return 0;
        }
        //Method to get document frequency having a particular term
        public int GetDocumentFrequency(string term)
        {
            if (documentFrequencies.ContainsKey(term))
            {
                return documentFrequencies[term];
            }
            return 0;
        }
        //Method to calculate term frequency
         private void UpdateTermFrequencies(string documentId, string[] terms)
        {
            foreach (string term in terms)
            {
                if (!termFrequencies.ContainsKey(term))
                {
                    termFrequencies[term] = new Dictionary<string, int>();
                }

                if (!termFrequencies[term].ContainsKey(documentId))
                {
                    termFrequencies[term][documentId] = 0;
                }

                termFrequencies[term][documentId]++;
            }
        }
        //method to calculate Document frequency
        private void UpdateDocumentFrequencies(string[] terms)
        {
            HashSet<string> uniqueTerms = new HashSet<string>(terms);

            foreach (string term in uniqueTerms)
            {
                if (!documentFrequencies.ContainsKey(term))
                {
                    documentFrequencies[term] = 0;
                }

                documentFrequencies[term]++;
            }
        }
        public int GetTotalDocuments()
        {
            return invertedIndex.Count; // or any other logic to determine the total number of documents
        }
    }
}
