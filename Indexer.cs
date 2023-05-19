namespace SearchAPI
{
     //Indexer class 
     public class Indexer{
        private Dictionary<string, HashSet<Document>> invertedIndex;
        private Dictionary<string, Dictionary<string, int>> termFrequencies;
        private Dictionary<string, int> documentFrequencies;

 
        public Indexer()
        {
            invertedIndex = new Dictionary<string, HashSet<Document>>();
            termFrequencies = new Dictionary<string, Dictionary<string, int>>();
            documentFrequencies = new Dictionary<string, int>();
        }
        //Method to Add Doc to Index
        public void AddIndex(Dictionary<string, int> wordsCount, Document document){
            foreach(string word in wordsCount.Keys){
                if(!invertedIndex.ContainsKey(word)){
                    invertedIndex[word] = new HashSet<Document>();
                }
                invertedIndex[word].Add(document);
            }
        }
        //Method to search and return query result
        public HashSet<Document> SearchIndex(string query)
        {
            HashSet<Document> result = new HashSet<Document>();
            if (invertedIndex.ContainsKey(query))
            {
                result = invertedIndex[query];
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