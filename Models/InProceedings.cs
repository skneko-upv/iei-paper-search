using System.Diagnostics.CodeAnalysis;

namespace IEIPaperSearch.Models
{
    /// <summary>
    /// An entry of a conference proceeding, this is, an academic paper published during the context of an academic conference or workshop.
    /// </summary>
    public class InProceedings : Submission
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

        public bool Equals([AllowNull] InProceedings other) => base.Equals(other);

        public override string ToString() =>
            $"InPr {base.ToString()}; pp. {StartPage}-{EndPage}, {Conference} {Edition}";

        public override bool Equals(object? obj) => Equals(obj as InProceedings);

        public override int GetHashCode() => base.GetHashCode();
    }
}
