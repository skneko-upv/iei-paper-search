#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

namespace IEIPaperSearch.Dto
{
    public class BookDto : SubmissionDto
    {
        /// <summary>
        /// The entity that has published this book.
        /// </summary>
        public string Publisher { get; set; }
    }
}
