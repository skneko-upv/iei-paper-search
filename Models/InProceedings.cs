namespace IEIPaperSearch.Models
{
    public class InProceedings : Submission  // comunicación congreso
    {
        public string Conference { get; set; }
        public string? Edition { get; set; }
        public string? StartPage { get; set; }
        public string? EndPage { get; set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private InProceedings() : base()
        { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public InProceedings(string title, int year, string? url, string conference, string? edition, string? startPage, string? endPage) : base(title, year, url)
        {
            Conference = conference;
            Edition = edition;
            StartPage = startPage;
            EndPage = endPage;
        }
    }
}
