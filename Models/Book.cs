namespace IEIPaperSearch.Models
{
    public class Book : Submission
    {
        public string Publisher { get; set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private Book() : base()
        { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public Book(string title, int year, string url, string publisher) : base(title, year, url)
        {
            Publisher = publisher;
        }
    }
}
