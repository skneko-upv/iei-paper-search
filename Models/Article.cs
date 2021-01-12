#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IEIPaperSearch.Models
{
    /// <summary>
    /// An academic article, usually published in a journal.
    /// </summary>
    public class Article : Submission, IEquatable<Article>
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
        public Issue? PublishedIn { get; set; }

        private Article() : base()
        { }

        public Article(string title, int year, string? url, string? startPage, string? endPage) : base(title, year, url)
        {
            StartPage = startPage;
            EndPage = endPage;
        }

        public bool Equals([AllowNull] Article other) => base.Equals(other);

        public override string ToString() =>
            $"Arti {base.ToString()}; pp. {StartPage}~{EndPage}, {PublishedIn}";

        public override bool Equals(object? obj) => Equals(obj as Article);

        public override int GetHashCode() => base.GetHashCode();
    }
}
