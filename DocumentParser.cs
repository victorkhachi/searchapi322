namespace SearchAPI{
    public class DocumentParser{
        private string docContent;
        private List<string> StopWords = new List<string>{"the", "a", "an", "and", "but","or", "for", "nor", "on", "at", "to", "from", "by", "with", "in", "of", "am", "I", "i", "my", "our", "has", "can", "been", "have", "that", "is", "isn't", "was", "that", "those", "these", "you", "me"};

        private string StemWord(string word){
            var stemmer = new Stemmer();
            return stemmer.Stem(word.ToLowerInvariant());
        }

        public DocumentParser(Document document){
            docContent = document.GetContent();
        }
        
        public List<string> GetWords()
        {
            //Tokenizes the docContent into words
            string[] allWords = docContent.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries); 

            //Stem words in doc
            for (int i = 0; i < allWords.Length; i++)
            {
                allWords[i] = StemWord(allWords[i]);
                allWords[i] = new string(allWords[i].Where(c => char.IsLetterOrDigit(c)).ToArray()).ToLower();
            }
            
            Array.Sort(allWords);//Sorts words

            // Removes StopWords
            var filteredWords = allWords.Where(word => !StopWords.Contains(word)).ToList(); 

            //returns list of words
            return filteredWords;
        }


        public Dictionary<string, int> Parse(){
            var parseWords = GetWords();
            
            //Computes the frequency of each word in Doc
            var frequency = new Dictionary<string, int>();

            foreach (var word in parseWords){
                if (!StopWords.Contains(word)){
                    if (frequency.ContainsKey(word)){
                        frequency[word]++;
                    } else{
                        frequency[word] = 1;
                    }
                }
            }
            //returns Dictionary of words and their frequency in doc
            return frequency;
        }
    }
}