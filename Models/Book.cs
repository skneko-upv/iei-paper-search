namespace IEIPaperSearch.Models
{
    public class Book : Submission
    {
        public string Publisher { get; set; }

        public Book(string title, int year, string url, string publisher) : base(title, year, url)
        {
            Publisher = publisher;
        }
    }
}
