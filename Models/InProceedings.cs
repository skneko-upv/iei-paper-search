namespace IEIPaperSearch.Models
{
    public class InProceedings : Submission  // comunicación congreso
    {
        public string Conference { get; set; }
        public string Edition { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }

        public InProceedings(string title, int year, string url, string conference, string edition, int startPage, int endPage) : base(title, year, url)
        {
            Conference = conference;
            Edition = edition;
            StartPage = startPage;
            EndPage = endPage;
        }
    }
}
