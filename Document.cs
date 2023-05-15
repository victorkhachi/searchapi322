namespace SearchAPI{
    public class Document{
        private int ID;
        private string Title;
        private string Content;
        private string URL;

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
        public override string ToString()
        {
            return $"ID: {this.ID}\nTitle: {this.Title}\nCONTENT: {this.Content}\nURL: {this.URL}";
        }
    }
}