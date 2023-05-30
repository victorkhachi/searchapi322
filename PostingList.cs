namespace SearchAPI
{
    public class Posting
    {
        public int DocumentId { get; set; }
        public int Frequency { get; set; }

        public Posting(int documentId, int frequency)
        {
            DocumentId = documentId;
            Frequency = frequency;
        }
    }
    public class PostingList
    {
        private List<Posting> postings;

        public PostingList()
        {
            postings = new List<Posting>();
        }

        public void AddPosting(int documentId, int frequency)
        {
            var posting = new Posting(documentId, frequency);
            postings.Add(posting);
        }
        
        public List<Posting> GetPostings()
        {
            return postings;
        }
    }
}