namespace _322SearchAPI;

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SearchAPI;

public partial class Form1 : Form
{
    private Indexer indexer;
    // Define controls for the first page
      private TextBox textBoxSearch = new TextBox();
      private Button buttonSearch = new Button();
      private Label labelTitle = new Label();
      private Query searchQuery;
      private Ranking ranking;
      private List<SearchResult> rankedResults;

     // Define controls for the second page
      private ListView listViewResults = new ListView();

    public Form1()
    {
        InitializeComponent();
        //Create an instance of the Document class
        Document document = new Document(0, "Title", "Content", "URL");
        // Create an instance of the DocumentParser class
        DocumentParser documentParser = new DocumentParser(document);
        DBrepository dbRepository = new DBrepository("string", "databaseName"); // Create an instance of DBrepository

        // Initialize the indexer
        indexer = new Indexer(documentParser, dbRepository);
        searchQuery = new Query("", indexer); // Initialize the search query
        ranking = new Ranking(searchQuery, indexer); // Initialize the ranking
        rankedResults = new List<SearchResult>(); // Initialize the ranked results

    }
    private void InitializeComponent()
    {
        // Create and configure controls for the first page
        textBoxSearch = new TextBox();
        buttonSearch = new Button();
        labelTitle = new Label();

        // Configure properties for controls
        textBoxSearch.Location = new System.Drawing.Point(100, 100);
        textBoxSearch.Size = new System.Drawing.Size(200, 20);

        buttonSearch.Location = new System.Drawing.Point(150, 150);
        buttonSearch.Size = new System.Drawing.Size(100, 30);
        buttonSearch.Text = "Search";
        buttonSearch.Click += ButtonSearch_Click;

        labelTitle.Location = new System.Drawing.Point(100, 50);
        labelTitle.Size = new System.Drawing.Size(200, 20);
        labelTitle.Text = "322SearchAPI";

        // Add controls to the form
        Controls.Add(textBoxSearch);
        Controls.Add(buttonSearch);
        Controls.Add(labelTitle);

        // Hide the controls for the second page initially
        // Define controls for the second page

        // Configure properties for controls
        listViewResults = new ListView();
        listViewResults.Location = new System.Drawing.Point(50, 50);
        listViewResults.Size = new System.Drawing.Size(300, 200);
        listViewResults.View = View.Details;

        // Add columns to the ListView control
        listViewResults.Columns.Add("Title");
        listViewResults.Columns.Add("Relevance Score");
        // ...

        // Add the ListView control to the form
        Controls.Add(listViewResults);

        this.SuspendLayout();

        // 
        // Form1
        // 
        this.ClientSize = new System.Drawing.Size(484, 361);
        this.Name = "Form1";
        this.ResumeLayout(false);
    }
    private void ButtonSearch_Click(object? sender, EventArgs e)
    {
        // Handle the search button click event
        // Get the query text from the text box
        string query = textBoxSearch.Text;
        // Call your backend logic here to perform the search query
         // Create an instance of the Query class
        Query searchQuery = new Query(query, indexer);
        // Execute the query and get the search results
        HashSet<Document> searchResults = searchQuery.Execute();
        // Create an instance of the Ranking class
        Ranking ranking = new Ranking(searchQuery, indexer);

        // Rank the search results
        List<SearchResult> rankedResults = ranking.RankResults(searchResults.ToList());
        // Clear the ListView control
        listViewResults.Items.Clear();

        // Populate the ListView control with the search results
        foreach (SearchResult result in rankedResults)
        {
            var listItem = new ListViewItem(new[] { result.Document.GetTitle(), result.RelevanceScore.ToString() });
            listViewResults.Items.Add(listItem);
        }

        // Display the search results on the second page
        // ...
        ShowResultsPage();
    }
    private void ShowResultsPage()
    {
        // Hide the first page (current form)
        this.Hide();

        // Create an instance of the second page (ResultsPage)
        ResultsPage resultsPage = new ResultsPage(rankedResults);
        resultsPage.FormClosed += ResultsPage_FormClosed;

        // Show the second page
        resultsPage.Show();
    }
    private void ResultsPage_FormClosed(object? sender, FormClosedEventArgs e)
    {
        this.Show();
    }

    //ResultsPage class
    public class ResultsPage : Form
    {
        // Add any necessary fields, properties, or methods for results page
        private ListBox listBoxResults;

        public ResultsPage(List<SearchResult> searchResults)
        {
            InitializeComponent();
            // Call a method to display the search results
            DisplayResults(searchResults);
            listBoxResults = new ListBox();
        }

        private void InitializeComponent()
        {
            // ...
            listBoxResults.Location = new System.Drawing.Point(50, 50);
            listBoxResults.Size = new System.Drawing.Size(300, 200);
            Controls.Add(listBoxResults);

            // ...
        }

        // Add event handlers, additional logic, and UI controls as needed
        private void DisplayResults(List<SearchResult> searchResults)
        {
            foreach (SearchResult document in searchResults)
            {
                string listItem = $"{document.Document}: {GetSnippet(document.Document.GetContent())}";
                listBoxResults.Items.Add(listItem);
            }
        }

        private string GetSnippet(string content)
        {
            const int MaxSnippetLength = 50; // Adjust the maximum length of the snippet as needed
            string snippet = content.Substring(0, Math.Min(content.Length, MaxSnippetLength));
            if (content.Length > MaxSnippetLength)
            snippet += "...";
            return snippet;
        }

    }
}
