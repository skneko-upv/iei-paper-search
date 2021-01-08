using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IEIPaperSearch.Models
{
    /// <summary>
    /// One of the releases of an academic journal.
    /// </summary>
    /// <remarks>
    /// Each journal has a different organization scheme for numbering its issues,
    /// and hence a different combination of the members of this class will be used
    /// for each one.
    /// </remarks>
    public class Issue : IEquatable<Issue>
    {
        public int Id { get; private set; }

        /// <summary>
        /// The volume number or identifier of this issue, in the context of its journal
        /// </summary>
        public string? Volume { get; set; }

        /// <summary>
        /// The unique number identifier of this issue, in the context of its journal.
        /// </summary>
        public string? Number { get; set; }

        /// <summary>
        /// The month in which this issue was released.
        /// </summary>
        public string? Month { get; set; }

        /// <summary>
        /// The journal this issue is a release of.
        /// </summary>
        public Journal Journal { get; set; }

        /// <summary>
        /// The academic articles publsihed in this issue.
        /// </summary>
        public ICollection<Article> Articles { get; set; } = new HashSet<Article>();

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private Issue()
        { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public Issue(string? volume, string? number, string? month, Journal journal)
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
