using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IEIPaperSearch.Models
{
    public abstract class Submission : IEquatable<Submission>
    {
        public Guid Id { get; private set; }

        public string Title { get; set; }
        public int Year { get; set; }
        public string? URL { get; set; }

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
    }
}
