#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

using System;
using System.Diagnostics.CodeAnalysis;

namespace IEIPaperSearch.Models
{
    public class Article : Submission, IEquatable<Article>
    {
        public int? StartPage { get; set; }
        public int? EndPage { get; set; }

        public Issue PublishedIn { get; set; }


        public Article(string title, int year, string url, int? startPage, int? endPage) : base(title, year, url)
        {
            StartPage = startPage;
            EndPage = endPage;
        }

        public bool Equals([AllowNull] Article other) => base.Equals(other);
    }
}
