using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IEIPaperSearch.Models
{
    public abstract class Submission : IEquatable<Submission>
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string URL { get; set; }

        public ICollection<Person> Authors = new HashSet<Person>();

        protected Submission(string title, int year, string url)
        {
            Title = title;
            Year = year;
            URL = url;
        }

        public bool Equals([AllowNull] Submission other)
            => (other != null) && (other.Title == Title) && (other.Year == Year);
    }
}
