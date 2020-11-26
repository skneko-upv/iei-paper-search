#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

using System;
using System.Diagnostics.CodeAnalysis;

namespace IEIPaperSearch.Models
{
    public class Article : Submission, IEquatable<Article>
    {
        public string? StartPage { get; set; }
        public string? EndPage { get; set; }

        public Issue PublishedIn { get; set; }

        private Article() : base()
        { }

        public Article(string title, int year, string? url, string? startPage, string? endPage) : base(title, year, url)
        {
            StartPage = startPage;
            EndPage = endPage;
        }

        public bool Equals([AllowNull] Article other) => base.Equals(other);
    }
}
