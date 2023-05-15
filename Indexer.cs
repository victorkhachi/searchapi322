namespace SearchAPI{
     //Indexer class 
     public class Indexer{
        private Dictionary<string, HashSet<Document>> invertedIndex;
 
        public Indexer()
        {
            invertedIndex = new Dictionary<string, HashSet<Document>>();
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
    }
}