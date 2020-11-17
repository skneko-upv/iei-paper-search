using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IEIPaperSearch.Models
{
    public class Issue : IEquatable<Issue>  // Ejemplar
    {
        public int Volume { get; set; }
        public int Number { get; set; }
        public int Month { get; set; }

        public Journal Journal { get; set; }
        public ICollection<Article> Articles { get; set; } = new HashSet<Article>();

        public Issue(int volume, int number, int month, Journal journal)
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
