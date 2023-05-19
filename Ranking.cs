namespace SearchAPI{
    public class Ranking
    {
        public List<SearchResult> RankResults(Query query, Indexer index, List<Document> documents)
        {
            List<SearchResult> results = new List<SearchResult>();

            foreach (Document document in documents)
            {
                double relevanceScore = CalculateRelevanceScore(query, index, document);
                results.Add(new SearchResult(document, relevanceScore));
            }

            // Sort the results based on relevance score in descending order
            results = results.OrderByDescending(r => r.RelevanceScore).ToList();

            return results;
        }

        private double CalculateRelevanceScore(Query query, Indexer indexer, Document document)
        {
            double relevanceScore = 0.0;

            foreach (string term in query.GetTerms())
            {
                double termFrequency = indexer.GetTermFrequency(term, document.GetID());
                double documentFrequency = indexer.GetDocumentFrequency(term);
                double inverseDocumentFrequency = Math.Log(indexer.GetTotalDocuments() / (documentFrequency + 1)); // Add 1 to avoid division by zero

                relevanceScore += termFrequency * inverseDocumentFrequency;
            }
            return relevanceScore;
        }
    }
}