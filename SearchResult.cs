namespace SearchAPI{
   public class SearchResult
    {
        public Document Document { get; }
        public double RelevanceScore { get; }

        public SearchResult(Document document, double relevanceScore)
        {
            Document = document;
            RelevanceScore = relevanceScore;
        }
    } 
}