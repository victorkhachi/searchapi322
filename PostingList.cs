namespace SearchAPI{
    public class PostingList
    {
       private List<Tuple<int, int>> postings;

       public PostingList()
       {
            postings = new List<Tuple<int, int>>();
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