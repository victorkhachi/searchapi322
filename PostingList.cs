namespace SearchAPI{
    public class PostingList
    {
       private List<Tuple<int, int>> postings;
       private string? Term;

        public PostingList(string term)
       {
            postings = new List<Tuple<int, int>>();
            this.Term = term;
       }
        public void AddPosting(int docId, int frequency)
        {
            postings.Add(new Tuple<int, int>(docId, frequency));
        }
        public List<Tuple<int, int>> GetPostings(){
            return postings;
        }
    }
}