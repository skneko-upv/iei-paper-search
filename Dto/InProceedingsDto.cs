#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

namespace IEIPaperSearch.Dto
{
    public class InProceedingsDto : SubmissionDto
    {
        /// <summary>
        /// The name of the conference or workshop associated to the containing proceeding.
        /// </summary>
        public string Conference { get; set; }

        /// <summary>
        /// The edition of the conference or workshop.
        /// </summary>
        public string? Edition { get; set; }

        /// <summary>
        /// The page of the proceedings this entry appears in, in which this
        /// entry starts.
        /// </summary>
        public string? StartPage { get; set; }

        /// <summary>
        /// The page of the proceedings this entry appears in, in which this
        /// entry ends.
        /// </summary>
        public string? EndPage { get; set; }
    }
}
