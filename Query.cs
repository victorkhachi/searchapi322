namespace SearchAPI{
    public class Query{
        private readonly string query;
        private List<string> StopWords = new List<string>{"the", "a", "an", "and", "but","or", "for", "nor", "on", "at", "to", "from", "by", "with", "in", "of", "am", "I", "i", "my", "our", "has", "can", "been", "have", "that", "is", "isn't", "was", "that", "those", "these", "you", "me"};

        public Query(string query){
            this.query = query;
        }
        public List<string> GetTerms(){
            string[] words = query.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries); 

             // Removes any alphanumeric char and convert to lowercase
            for(int i = 0; i < words.Length; i++){
                words[i] = new string(words[i].Where(c => char.IsLetterOrDigit(c)).ToArray()).ToLower();
            }

            // Removes StopWords
            var terms = words
            .Where(word => !StopWords.Contains(word)).ToList(); 

            //returns list of words
            return terms;
        }
        public HashSet<Document> Execute()
        {
            Indexer indexer = new Indexer();
            HashSet<Document> results = new HashSet<Document>();

            // get the list of query terms
            List<string> terms = GetTerms();

            // find the set of documents containing all query terms
            HashSet<Document>? intersection = null;
            foreach (string term in terms)
            {
                HashSet<Document> postingList = indexer.SearchIndex(term);
                if (postingList == null)
                {
                    return results;
                }

                if (intersection == null)
                {
                    intersection = postingList;
                }
                else
                {
                    intersection.IntersectWith(postingList);
                }
            }

            // retrieve the documents corresponding to the posting list entries
            if (intersection != null)
            {
                foreach (Document document in intersection)
                {
                    results.Add(document);
                }
            }

            return results;
        }

    }
}