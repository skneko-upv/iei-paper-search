#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

namespace IEIPaperSearch.Dto
{
    public class ArticleDto : SubmissionDto
    {
        /// <summary>
        /// The page of the journal this article was published in, in which this
        /// article starts.
        /// </summary>
        public string? StartPage { get; set; }

        /// <summary>
        /// The page of the journal this article was published in, in which this
        /// article ends.
        /// </summary>
        public string? EndPage { get; set; }

        /// <summary>
        /// The issue of the journal this article was published in.
        /// </summary>
        public IssueDto? PublishedIn { get; set; }
    }
}
