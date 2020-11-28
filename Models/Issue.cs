using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IEIPaperSearch.Models
{
    public class Issue : IEquatable<Issue>  // Ejemplar
    {
        public Guid Id { get; private set; }

        public string? Volume { get; set; }
        public string? Number { get; set; }
        public int? Month { get; set; }

        public Journal Journal { get; set; }
        public ICollection<Article> Articles { get; set; } = new HashSet<Article>();

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private Issue()
        { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public Issue(string? volume, string? number, int? month, Journal journal)
        {
            Volume = volume;
            Number = number;
            Month = month;
            Journal = journal;
        }

        public bool Equals([AllowNull] Issue other)
            => (other != null) && (other.Volume == Volume) && (other.Number == Number) && (other.Month == Month);
    }
}
