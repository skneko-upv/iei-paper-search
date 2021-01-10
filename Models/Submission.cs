using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IEIPaperSearch.Models
{
    /// <summary>
    /// An academic paper or publication, of some sort (article, book...), which is available for access online.
    /// </summary>
    public abstract class Submission : IEquatable<Submission>
    {
        public int Id { get; private set; }

        /// <summary>
        /// The title of this paper or publication.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The year in which this paper or publication was released to the public.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// A URL pointing to a readable digital upload of this paper or publication.
        /// </summary>
        public string? URL { get; set; }

        /// <summary>
        /// A collection of the people who have authored this paper or publication.
        /// </summary>
        public ICollection<Person> Authors = new HashSet<Person>();

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        protected Submission() 
        { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        protected Submission(string title, int year, string? url)
        {
            Title = title;
            Year = year;
            URL = url;
        }

        public bool Equals([AllowNull] Submission other)
            => (other != null) && (other.Title == Title) && (other.Year == Year);

        public override string ToString() =>
            $"{string.Join(",", Authors)} - {Title} ({Year}) - {URL}";

        public override bool Equals(object? obj) => Equals(obj as Submission);

        public override int GetHashCode() => HashCode.Combine(Title, Year);
    }
}
