namespace SearchAPI{
    public class Document{
        private int ID { get; set; }
        private string Title { get; set; }
        private string Content { get; set; }
        private string URL { get; set; }
        private double Score { get; set; }

        public Document(int ID, string Title, string Content, string URL){
            this.ID = ID;
            this.Content = Content;
            this.Title = Title;
            this.URL = URL;
        }
        public int GetID(){
            return this.ID;
        }
        public string GetTitle(){
            return this.Title;
        }
        public string GetContent(){
            return this.Content;
        }
        public string GetURL(){
            return this.URL;
        }
        public double GetScore(){
            return this.Score;
        }
        public override string ToString()
        {
            return $"ID: {this.ID}\nTitle: {this.Title}\nCONTENT: {this.Content}\nURL: {this.URL}\nScore: {this.Score}";
        }
    }
}