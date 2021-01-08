using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IEIPaperSearch.Models
{
    /// <summary>
    /// An academic journal, which emits issues where articles get published.
    /// </summary>
    public class Journal : IEquatable<Journal>
    {
        public int Id { get; private set; }

        /// <summary>
        /// The canonical name of this journal.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A collection of all releases known of this journal.
        /// </summary>
        public ICollection<Issue> Issues { get; set; } = new HashSet<Issue>();

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private Journal()
        { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public Journal(string name)
        {
            Name = name;
        }
        public bool Equals([AllowNull] Journal other)
            => (other != null) && (Name == other.Name);
    }
}
