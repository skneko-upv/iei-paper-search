using System.Diagnostics.CodeAnalysis;

namespace IEIPaperSearch.Models
{
    /// <summary>
    /// An academic book, usually printed whole instead of published inside a journal.
    /// </summary>
    public class Book : Submission
    {
        /// <summary>
        /// The entity that has published this book.
        /// </summary>
        public string Publisher { get; set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private Book() : base()
        { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public Book(string title, int year, string? url, string publisher) : base(title, year, url)
        {
            Publisher = publisher;
        }

        public bool Equals([AllowNull] Book other) => base.Equals(other);

        public override string ToString() =>
            $"Book {base.ToString()}; {Publisher}";

        public override bool Equals(object? obj) => Equals(obj as Book);

        public override int GetHashCode() => base.GetHashCode();
    }
}
