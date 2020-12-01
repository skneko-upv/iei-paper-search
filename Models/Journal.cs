using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IEIPaperSearch.Models
{
    public class Journal : IEquatable<Journal>  // Revista
    {
        public int Id { get; private set; }

        public string Name { get; set; }

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
