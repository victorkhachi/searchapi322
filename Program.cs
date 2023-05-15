//Omo, Na here I dey test the codes
namespace SearchAPI
{
    class Program
    {
        public static void Main(string[] args)
        {
            string text = "The Quick brown fox runs very fast and jumped over the Lazy Dog Very Quick, and he runs Quick, Quick, Quick, Quick, Quick, Quick.";

            string text1 = "Heyy, I am here today mainly to test my typing skill, it's been a while, I have been good though, work's been fine, school's been okay, my results has been dissappointing, Family has been fine and happy, reconnections has been happening, I talked to Rose and wisdom but it's different, I can obviously see I have changed or am I just lonely?";
            
            string text2 = "Forex has been on indefinitely leave for a while, I am halting on that front for now.I have to increase my income, and I have been thinking of a few things: - Affiliate marketing is the first plan and I have drafted out my plan, I really pray it would be a success - Web development is the next I have to kick off as soon as I am done with setting up all that is necessary for the first but this involves a lot of learning because I am gunning for full stack.- Relearning Forex skill, this will be a much later plan.God thank you for always being with me.";

            Document document = new Document(1 , "A text", text, "url");

            Document document1 = new Document(2 , "A text1", text1, "url1");

            Document document2 = new Document(3 , "A text2", text2, "url2");

            DocumentParser documentParser = new DocumentParser(document2);

             DocumentParser documentParser1 = new DocumentParser(document1);

            Indexer Index = new Indexer();

            Console.Clear();
            //using Document class
            Console.WriteLine(document2.GetContent());

            Dictionary<string, int> wordCount1 = documentParser1.Parse();
            //using DocumentParser class
            Dictionary<string, int> wordCounts = documentParser.Parse();
            foreach(KeyValuePair<string, int> pair in wordCounts){
                Console.WriteLine(pair.Key + ": " + pair.Value);
            }
            //using Indexer class
            Index.AddIndex(wordCounts, document2);
            Index.AddIndex(wordCount1, document1);
            var Query = Index.SearchIndex("stack");
            foreach(var result in Query){
                Console.WriteLine("DocId: "+ result.GetID()+" " +"Title Of Doc: " +result.GetTitle());
            }
        }
    }
}