#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

namespace IEIPaperSearch.Dto
{
    public class IssueDto
    {
        /// <summary>
        /// The volume number or identifier of this issue, in the context of its journal
        /// </summary>
        public string? Volume { get; set; }

        /// <summary>
        /// The unique number identifier of this issue, in the context of its journal.
        /// </summary>
        public string? Number { get; set; }

        /// <summary>
        /// The month in which this issue was released.
        /// </summary>
        public string? Month { get; set; }

        /// <summary>
        /// The journal this issue is a release of.
        /// </summary>
        public JournalDto Journal { get; set; }
    }
}
